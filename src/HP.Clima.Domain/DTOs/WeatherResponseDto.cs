using System.ComponentModel;

namespace HP.Clima.Domain.DTOs;

/// <summary>
/// DTO de resposta com informações meteorológicas completas
/// </summary>
public class WeatherResponseDto
{
    /// <summary>
    /// ID do CEP de origem (hash)
    /// </summary>
    /// <example>123456789</example>
    [Description("Identificador do CEP de origem")]
    public int SourceZipCodeId { get; set; }
    
    /// <summary>
    /// Localização geográfica associada
    /// </summary>
    [Description("Dados de localização")]
    public LocationDto Location { get; set; } = new();
    
    /// <summary>
    /// Condições meteorológicas atuais
    /// </summary>
    [Description("Clima atual")]
    public CurrentWeatherDto Current { get; set; } = new();
    
    /// <summary>
    /// Previsão diária para os próximos dias
    /// </summary>
    [Description("Previsão diária")]
    public List<DailyWeatherDto> Daily { get; set; } = new();
    
    /// <summary>
    /// Provedor da API meteorológica (open-meteo ou openweathermap)
    /// </summary>
    /// <example>open-meteo</example>
    [Description("Provedor de dados meteorológicos")]
    public string Provider { get; set; } = string.Empty;
}

/// <summary>
/// Condições meteorológicas atuais
/// </summary>
public class CurrentWeatherDto
{
    /// <summary>
    /// Temperatura atual em graus Celsius
    /// </summary>
    /// <example>23.5</example>
    [Description("Temperatura em °C")]
    public double TemperatureC { get; set; }
    
    /// <summary>
    /// Umidade relativa do ar (0-1)
    /// </summary>
    /// <example>0.65</example>
    [Description("Umidade relativa (0-1)")]
    public double Humidity { get; set; }
    
    /// <summary>
    /// Sensação térmica em graus Celsius
    /// </summary>
    /// <example>22.8</example>
    [Description("Sensação térmica em °C")]
    public double ApparentTemperatureC { get; set; }
    
    /// <summary>
    /// Data e hora da observação
    /// </summary>
    /// <example>2025-11-20T14:00:00</example>
    [Description("Data/hora da observação")]
    public DateTime ObservedAt { get; set; }
}

/// <summary>
/// Previsão diária de temperatura
/// </summary>
public class DailyWeatherDto
{
    /// <summary>
    /// Data da previsão (formato ISO 8601)
    /// </summary>
    /// <example>2025-11-20</example>
    [Description("Data da previsão")]
    public string Date { get; set; } = string.Empty;
    
    /// <summary>
    /// Temperatura mínima do dia em graus Celsius
    /// </summary>
    /// <example>15.2</example>
    [Description("Temperatura mínima em °C")]
    public double TempMinC { get; set; }
    
    /// <summary>
    /// Temperatura máxima do dia em graus Celsius
    /// </summary>
    /// <example>28.7</example>
    [Description("Temperatura máxima em °C")]
    public double TempMaxC { get; set; }
}
