using HP.Clima.Domain.DTOs;
using HP.Clima.Domain.Mappers;
using HP.Clima.Domain.Repositories;
using HP.Clima.Domain.Interfaces.Services;
using HP.Clima.Domain.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using HP.Clima.Service.Handlers.Interfaces;

namespace HP.Clima.Service.Services;

public class CepService(
    IEnumerable<ICepApiHandler> cepApiHandlers,
    IZipCodeRepository zipCodeRepository,
    IValidationService validationService,
    IValidator<CepRequestDto> validator,
    ILogger<CepService> logger) : ICepService
{
    private readonly IEnumerable<ICepApiHandler> _cepApiHandlers = cepApiHandlers;
    private readonly IZipCodeRepository _zipCodeRepository = zipCodeRepository;
    private readonly IValidationService _validationService = validationService;
    private readonly IValidator<CepRequestDto> _validator = validator;
    private readonly ILogger<CepService> _logger = logger;

    public async Task<ZipCodeDto?> GetCepInfoAsync(string zipCode)
    {
        CepRequestDto cepRequest = new(zipCode);
        
        await _validationService.ValidateAsync(cepRequest, _validator, $"/api/cep/{zipCode}");

        var normalizedZipCode = cepRequest.GetNormalizedZipCode();

        var existingZipCode = await GetFromCacheAsync(normalizedZipCode);
        if (existingZipCode != null)
        {
            return existingZipCode;
        }

        var zipCodeDto = await TryGetFromExternalApisAsync(normalizedZipCode);
        
        if (zipCodeDto != null)
        {
            await SaveToCacheAsync(zipCodeDto);
            return zipCodeDto;
        }

        _logger.LogError("CEP {ZipCode} não encontrado em nenhuma API disponível", normalizedZipCode);
        throw new NotFoundException(
            detail: $"CEP {zipCode} não encontrado nas bases de dados consultadas.",
            instance: $"/api/cep/{zipCode}"
        );
    }

    public async Task<ZipCodeDto> SaveCepInfoAsync(CepRequestDto cepRequest)
    {
        await _validationService.ValidateAsync(cepRequest, _validator, "/api/cep");

        var normalizedZipCode = cepRequest.GetNormalizedZipCode();

        var existingZipCode = await _zipCodeRepository.GetByZipCodeAsync(normalizedZipCode);
        if (existingZipCode != null)
        {
            _logger.LogWarning("CEP {ZipCode} já existe no banco de dados", normalizedZipCode);
            throw new ConflictException(
                detail: $"CEP {cepRequest.ZipCode} já está cadastrado no sistema.",
                instance: "/api/cep"
            );
        }

        var zipCodeDto = await GetCepInfoAsync(cepRequest.ZipCode);
        
        return zipCodeDto!;
    }

    private async Task<ZipCodeDto?> GetFromCacheAsync(string normalizedZipCode)
    {
        var existingZipCode = await _zipCodeRepository.GetByZipCodeAsync(normalizedZipCode);
        
        if (existingZipCode != null)
        {
            _logger.LogInformation("CEP {ZipCode} encontrado no cache local", normalizedZipCode);
            return existingZipCode.ToDto();
        }

        return null;
    }

    private async Task<ZipCodeDto?> TryGetFromExternalApisAsync(string normalizedZipCode)
    {
        foreach (var handler in _cepApiHandlers)
        {
            var (success, data) = await handler.TryGetCepAsync(normalizedZipCode);
            
            if (success && data != null)
            {
                _logger.LogInformation("CEP {ZipCode} obtido com sucesso via {ApiName}", 
                    normalizedZipCode, handler.ApiName);
                return data;
            }
            
            _logger.LogWarning("{ApiName} falhou para CEP {ZipCode}. Tentando próxima API...", 
                handler.ApiName, normalizedZipCode);
        }

        return null;
    }

    private async Task SaveToCacheAsync(ZipCodeDto zipCodeDto)
    {
        try
        {
            var zipCodeEntity = zipCodeDto.ToEntity();
            await _zipCodeRepository.CreateAsync(zipCodeEntity);
            _logger.LogInformation("CEP {ZipCode} salvo com sucesso no cache", zipCodeDto.ZipCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao salvar CEP {ZipCode} no cache: {Message}", 
                zipCodeDto.ZipCode, ex.Message);
        }
    }
}
