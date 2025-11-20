using HP.Clima.Domain.DTOs;
using HP.Clima.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HP.Clima.API.Controllers;

/// <summary>
/// Endpoints para consulta de informações meteorológicas
/// </summary>
[ApiController]
[Route("api/weather")]
[Produces("application/json")]
public class WeatherController(IWeatherService weatherService) : ControllerBase
{
    private readonly IWeatherService _weatherService = weatherService;

    /// <summary>
    /// Consulta previsão do tempo para todos os CEPs cadastrados
    /// </summary>
    /// <param name="days">Número de dias de previsão (1-7). Padrão: 3</param>
    /// <returns>Lista com previsão do tempo para cada CEP cadastrado</returns>
    /// <response code="200">Previsão retornada com sucesso</response>
    /// <response code="400">Parâmetro 'days' inválido (deve ser entre 1 e 7)</response>
    /// <response code="404">Nenhum CEP cadastrado na base de dados</response>
    /// <response code="500">Erro interno do servidor</response>
    /// <remarks>
    /// Consulta as condições meteorológicas atuais e previsão diária para os próximos N dias.
    /// 
    /// Os dados são obtidos de APIs externas (OpenMeteo ou OpenWeatherMap) usando as
    /// coordenadas geográficas dos CEPs previamente cadastrados.
    /// 
    /// Exemplo de requisição:
    /// 
    ///     GET /api/weather?days=3
    ///     
    /// Exemplo de resposta (200 OK):
    /// 
    ///     [
    ///       {
    ///         "sourceZipCodeId": 123456789,
    ///         "location": {
    ///           "lat": -23.5475,
    ///           "lon": -46.6361,
    ///           "city": "São Paulo",
    ///           "state": "SP"
    ///         },
    ///         "current": {
    ///           "temperatureC": 23.5,
    ///           "humidity": 0.65,
    ///           "apparentTemperatureC": 22.8,
    ///           "observedAt": "2025-11-20T14:00:00"
    ///         },
    ///         "daily": [
    ///           {
    ///             "date": "2025-11-20",
    ///             "tempMinC": 15.2,
    ///             "tempMaxC": 28.7
    ///           },
    ///           {
    ///             "date": "2025-11-21",
    ///             "tempMinC": 16.0,
    ///             "tempMaxC": 29.3
    ///           },
    ///           {
    ///             "date": "2025-11-22",
    ///             "tempMinC": 17.1,
    ///             "tempMaxC": 30.2
    ///           }
    ///         ],
    ///         "provider": "open-meteo"
    ///       }
    ///     ]
    ///     
    /// </remarks>
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
