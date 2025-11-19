using HP.Clima.Domain.DTOs;
using HP.Clima.Domain.Mappers;
using HP.Clima.Service.Proxies.ViaCepApi;
using Microsoft.Extensions.Logging;
using Refit;

namespace HP.Clima.Service.Handlers;

public class ViaCepHandler(IViaCepApiProxy viaCepApiProxy, ILogger<ViaCepHandler> logger) : ICepApiHandler
{
    private readonly IViaCepApiProxy _viaCepApiProxy = viaCepApiProxy;
    private readonly ILogger<ViaCepHandler> _logger = logger;

    public string ApiName => "ViaCEP";

    public async Task<(bool Success, ZipCodeDto? Data)> TryGetCepAsync(string normalizedZipCode)
    {
        try
        {
            _logger.LogInformation("[{ApiName}] Consultando CEP {ZipCode}", ApiName, normalizedZipCode);
            
            var response = await _viaCepApiProxy.GetCepWithResponseAsync(normalizedZipCode);
            
            if (response.IsSuccessStatusCode && response.Content != null)
            {
                if (!string.IsNullOrEmpty(response.Content.Erro))
                {
                    _logger.LogWarning("[{ApiName}] CEP {ZipCode} não encontrado (campo 'erro' presente)", 
                        ApiName, normalizedZipCode);
                    return (false, null);
                }

                var zipCodeDto = response.Content.ViaCepResponseToDto();
                _logger.LogInformation("[{ApiName}] CEP {ZipCode} encontrado com sucesso", ApiName, normalizedZipCode);
                return (true, zipCodeDto);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("[{ApiName}] CEP {ZipCode} não encontrado (404)", ApiName, normalizedZipCode);
                return (false, null);
            }

            _logger.LogWarning("[{ApiName}] Resposta com status {StatusCode} para CEP {ZipCode}", 
                ApiName, response.StatusCode, normalizedZipCode);
            return (false, null);
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, "[{ApiName}] Erro ao consultar CEP {ZipCode}: {Message}", 
                ApiName, normalizedZipCode, ex.Message);
            return (false, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[{ApiName}] Erro inesperado ao consultar CEP {ZipCode}: {Message}", 
                ApiName, normalizedZipCode, ex.Message);
            return (false, null);
        }
    }
}
