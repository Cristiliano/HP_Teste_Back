using HP.Clima.Domain.DTOs;
using HP.Clima.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HP.Clima.API.Controllers;

[ApiController]
[Route("api/weather")]
public class WeatherController(IWeatherService weatherService) : ControllerBase
{
    private readonly IWeatherService _weatherService = weatherService;

    [HttpGet]
    [ProducesResponseType(typeof(List<WeatherResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWeather([FromQuery] int days = 3)
    {
        var result = await _weatherService.GetWeatherAsync(new(days));
        return Ok(result);
    }
}
