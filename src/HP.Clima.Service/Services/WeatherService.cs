using HP.Clima.Domain.DTOs;
using HP.Clima.Domain.Entities;
using HP.Clima.Domain.Exceptions;
using HP.Clima.Domain.Interfaces.Services;
using HP.Clima.Domain.Repositories;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using HP.Clima.Service.Handlers.Interfaces;

namespace HP.Clima.Service.Services;

public class WeatherService(
    IEnumerable<IWeatherApiHandler> weatherApiHandlers,
    IZipCodeRepository zipCodeRepository,
    IValidationService validationService,
    IValidator<WeatherRequestDto> validator,
    IMemoryCache memoryCache,
    ILogger<WeatherService> logger) : IWeatherService
{
    private readonly IEnumerable<IWeatherApiHandler> _weatherApiHandlers = weatherApiHandlers;
    private readonly IZipCodeRepository _zipCodeRepository = zipCodeRepository;
    private readonly IValidationService _validationService = validationService;
    private readonly IValidator<WeatherRequestDto> _validator = validator;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly ILogger<WeatherService> _logger = logger;

    public async Task<List<WeatherResponseDto>> GetWeatherAsync(WeatherRequestDto request)
    {
        await _validationService.ValidateAsync(request, _validator, "/api/weather");

        var zipCodes = await _zipCodeRepository.GetAllAsync();
        var sortedZipCodes = zipCodes.OrderByDescending(x => x.CreatedAtUtc).ToList();

        if (sortedZipCodes.Count == 0)
        {
            _logger.LogWarning("Nenhum CEP encontrado no banco de dados");
            throw new NotFoundException(
                detail: "Nenhum CEP foi salvo ainda no sistema. Por favor, cadastre um CEP antes de consultar o clima.",
                instance: "/api/weather",
                errorCode: "no-saved-cep"
            );
        }

        var weatherResponses = new List<WeatherResponseDto>();

        foreach (var zipCode in sortedZipCodes)
        {
            var weatherData = await GetWeatherForZipCodeAsync(zipCode, request.Days);
            if (weatherData != null)
            {
                weatherResponses.Add(weatherData);
            }
        }

        return weatherResponses;
    }

    private async Task<WeatherResponseDto?> GetWeatherForZipCodeAsync(ZipCodeEntity zipCode, int days)
    {
        var cacheKey = GenerateCacheKey(zipCode, days);
        if (_memoryCache.TryGetValue(cacheKey, out WeatherResponseDto? cachedWeather))
        {
            _logger.LogInformation("Clima encontrado no cache para {CacheKey}", cacheKey);
            return cachedWeather;
        }

        var weatherData = await TryGetFromExternalApisAsync(zipCode, days);
        
        if (weatherData != null)
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
            _memoryCache.Set(cacheKey, weatherData, cacheOptions);
            _logger.LogInformation("Clima salvo no cache para {CacheKey}", cacheKey);
        }

        return weatherData;
    }

    private async Task<WeatherResponseDto?> TryGetFromExternalApisAsync(ZipCodeEntity zipCode, int days)
    {
        foreach (var handler in _weatherApiHandlers)
        {
            var (success, data) = await handler.TryGetWeatherAsync(zipCode, days);

            if (success && data != null)
            {
                _logger.LogInformation("Clima obtido com sucesso via {ApiName} para {City}, {State}", 
                    handler.ApiName, zipCode.City, zipCode.State);
                return data;
            }

            _logger.LogWarning("{ApiName} falhou para {City}, {State}. Tentando próxima API...", 
                handler.ApiName, zipCode.City, zipCode.State);
        }

        _logger.LogWarning("Não foi possível obter clima para {City}, {State} de nenhuma API", 
            zipCode.City, zipCode.State);
            
        return null;
    }

    private static string GenerateCacheKey(ZipCodeEntity zipCode, int days)
    {
        if (zipCode.Lat.HasValue && zipCode.Lon.HasValue)
        {
            return $"weather:{zipCode.Lat:F4}:{zipCode.Lon:F4}:{days}";
        }
        
        return $"weather:{zipCode.City}:{zipCode.State}:{days}";
    }
}
