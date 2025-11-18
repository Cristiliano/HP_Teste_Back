using HP.Clima.Domain.DTOs;
using HP.Clima.Domain.Mappers;
using HP.Clima.Domain.Repositories;
using HP.Clima.Domain.Interfaces.Services;
using HP.Clima.Service.Proxies.BrasilApi;
using Refit;

namespace HP.Clima.Service.Services;

public class CepService(IBrasilApiProxy brasilApiProxy, IZipCodeRepository zipCodeRepository) : ICepService
{
    private readonly IBrasilApiProxy _brasilApiProxy = brasilApiProxy;
    private readonly IZipCodeRepository _zipCodeRepository = zipCodeRepository;

    public async Task<ZipCodeDto?> GetCepInfoAsync(string zipCode)
    {
        var existingZipCode = await _zipCodeRepository.GetByZipCodeAsync(zipCode);
        if (existingZipCode != null)
        {
            return existingZipCode.ToDto();
        }

        try
        {
            var response = await _brasilApiProxy.GetCepWithResponseAsync(zipCode);
            
            if (response.IsSuccessStatusCode && response.Content != null)
            {
                var zipCodeDto = response.Content.BrasilApiCepResponseToDto();
                
                var zipCodeEntity = zipCodeDto.ToEntity();
                await _zipCodeRepository.CreateAsync(zipCodeEntity);
                
                return zipCodeDto;
            }
        }
        catch (ApiException ex)
        {
            Console.WriteLine($"Erro ao consultar CEP {zipCode}: {ex.Message}");
        }

        return null;
    }
}