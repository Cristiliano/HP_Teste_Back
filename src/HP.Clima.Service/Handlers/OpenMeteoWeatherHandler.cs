using HP.Clima.Domain.DTOs;
using HP.Clima.Domain.Entities;
using HP.Clima.Domain.Mappers;
using HP.Clima.Service.Handlers.Interfaces;
using HP.Clima.Service.Proxies.OpenMeteo;
using Microsoft.Extensions.Logging;
using Refit;

namespace HP.Clima.Service.Handlers;

public class OpenMeteoWeatherHandler(
    IOpenMeteoForecastProxy forecastProxy,
    IOpenMeteoGeocodingProxy geocodingProxy,
    ILogger<OpenMeteoWeatherHandler> logger) : IWeatherApiHandler
{
    private readonly IOpenMeteoForecastProxy _forecastProxy = forecastProxy;
    private readonly IOpenMeteoGeocodingProxy _geocodingProxy = geocodingProxy;
    private readonly ILogger<OpenMeteoWeatherHandler> _logger = logger;

    public string ApiName => "OpenMeteo";

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
                _logger.LogInformation("[{ApiName}] Lat/Lon não encontrados. Geocodificando {City}, {State}", 
                    ApiName, zipCodeEntity.City, zipCodeEntity.State);
                
                var (geocoded, geocodedLat, geocodedLon) = await TryGeocodeAsync(zipCodeEntity.City, zipCodeEntity.State);
                
                if (!geocoded)
                {
                    _logger.LogWarning("[{ApiName}] Não foi possível geocodificar {City}, {State}", 
                        ApiName, zipCodeEntity.City, zipCodeEntity.State);
                    return (false, null);
                }
                
                lat = geocodedLat;
                lon = geocodedLon;
            }

            _logger.LogInformation("[{ApiName}] Consultando clima para Lat: {Lat}, Lon: {Lon}, Days: {Days}", 
                ApiName, lat, lon, days);

            var response = await _forecastProxy.GetForecastAsync(
                latitude: lat,
                longitude: lon,
                current: "temperature_2m,relative_humidity_2m,apparent_temperature",
                daily: "temperature_2m_max,temperature_2m_min",
                forecastDays: days);

            if (response.IsSuccessStatusCode && response.Content != null)
            {
                var weatherDto = response.Content.OpenMeteoToDto(zipCodeEntity, lat, lon);
                _logger.LogInformation("[{ApiName}] Clima obtido com sucesso", ApiName);
                return (true, weatherDto);
            }

            _logger.LogWarning("[{ApiName}] Resposta com status {StatusCode}", 
                ApiName, response.StatusCode);
            return (false, null);
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

    private async Task<(bool Success, double Lat, double Lon)> TryGeocodeAsync(string city, string state)
    {
        try
        {
            var response = await _geocodingProxy.SearchLocationAsync(city);

            if (response.IsSuccessStatusCode && 
                response.Content != null && 
                response.Content.Results.Count > 0)
            {
                var result = response.Content.Results[0];
                _logger.LogInformation("[{ApiName}] Geocodificação bem-sucedida: Lat {Lat}, Lon {Lon}", 
                    ApiName, result.Latitude, result.Longitude);
                return (true, result.Latitude, result.Longitude);
            }

            return (false, 0, 0);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[{ApiName}] Erro ao geocodificar {City}, {State}: {Message}", 
                ApiName, city, state, ex.Message);
            return (false, 0, 0);
        }
    }
}
