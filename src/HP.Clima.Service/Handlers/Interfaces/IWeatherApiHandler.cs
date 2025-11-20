using HP.Clima.Domain.DTOs;
using HP.Clima.Domain.Entities;

namespace HP.Clima.Service.Handlers.Interfaces;

public interface IWeatherApiHandler
{
    string ApiName { get; }
    Task<(bool Success, WeatherResponseDto? Data)> TryGetWeatherAsync(
        ZipCodeEntity zipCodeEntity, 
        int days);
}
