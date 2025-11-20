using HP.Clima.Domain.DTOs;

namespace HP.Clima.Domain.Interfaces.Services;

public interface IWeatherService
{
    Task<List<WeatherResponseDto>> GetWeatherAsync(WeatherRequestDto request);
}
