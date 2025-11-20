using HP.Clima.Domain.DTOs;
using HP.Clima.Domain.Entities;
using HP.Clima.Domain.Mappers;
using HP.Clima.Service.Proxies.OpenWeatherMap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Refit;

namespace HP.Clima.Service.Handlers;

public class OpenWeatherMapHandler(
    IOpenWeatherMapProxy openWeatherMapProxy,
    IConfiguration configuration,
    ILogger<OpenWeatherMapHandler> logger) : IWeatherApiHandler
{
    private readonly IOpenWeatherMapProxy _openWeatherMapProxy = openWeatherMapProxy;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<OpenWeatherMapHandler> _logger = logger;

    public string ApiName => "OpenWeatherMap";

    public async Task<(bool Success, WeatherResponseDto? Data)> TryGetWeatherAsync(
        ZipCodeEntity zipCodeEntity, 
        int days)
    {
        try
        {
            double lat = zipCodeEntity.Lat ?? 0;
            double lon = zipCodeEntity.Lon ?? 0;

            if (lat == 0 || lon == 0)
            {
                _logger.LogWarning("[{ApiName}] Lat/Lon não disponíveis para {City}, {State}. Pulando este handler.", 
                    ApiName, zipCodeEntity.City, zipCodeEntity.State);
                return (false, null);
            }

            var apiKey = _configuration["HttpClients:OpenWeatherMap:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                _logger.LogError("[{ApiName}] API Key não configurada em HttpClients:OpenWeatherMap:ApiKey", ApiName);
                return (false, null);
            }

            _logger.LogInformation("[{ApiName}] Consultando clima para Lat: {Lat}, Lon: {Lon}, Days: {Days}", 
                ApiName, lat, lon, days);

            var currentResponse = await _openWeatherMapProxy.GetCurrentWeatherAsync(
                latitude: lat,
                longitude: lon,
                apiKey: apiKey);

            if (!currentResponse.IsSuccessStatusCode || currentResponse.Content == null)
            {
                _logger.LogWarning("[{ApiName}] Falha ao obter clima atual. Status: {StatusCode}", 
                    ApiName, currentResponse.StatusCode);
                return (false, null);
            }

            var forecastResponse = await _openWeatherMapProxy.GetForecastAsync(
                latitude: lat,
                longitude: lon,
                apiKey: apiKey);

            if (!forecastResponse.IsSuccessStatusCode || forecastResponse.Content == null)
            {
                _logger.LogWarning("[{ApiName}] Falha ao obter previsão. Status: {StatusCode}", 
                    ApiName, forecastResponse.StatusCode);
                return (false, null);
            }

            var weatherDto = currentResponse.Content.OpenWeatherMapToDto(
                forecastResponse.Content, 
                zipCodeEntity, 
                lat, 
                lon, 
                days);
            
            _logger.LogInformation("[{ApiName}] Clima obtido com sucesso", ApiName);
            return (true, weatherDto);
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, "[{ApiName}] Erro ao consultar clima: {Message}", 
                ApiName, ex.Message);
            return (false, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[{ApiName}] Erro inesperado ao consultar clima: {Message}", 
                ApiName, ex.Message);
            return (false, null);
        }
    }
}
