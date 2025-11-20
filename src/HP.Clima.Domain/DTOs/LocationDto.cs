using System.ComponentModel;

namespace HP.Clima.Domain.DTOs;

/// <summary>
/// Informações de localização geográfica
/// </summary>
public class LocationDto
{
    /// <summary>
    /// Latitude em graus decimais
    /// </summary>
    /// <example>-23.5475</example>
    [Description("Latitude")]
    public double Lat { get; set; }
    
    /// <summary>
    /// Longitude em graus decimais
    /// </summary>
    /// <example>-46.6361</example>
    [Description("Longitude")]
    public double Lon { get; set; }
    
    /// <summary>
    /// Nome da cidade
    /// </summary>
    /// <example>São Paulo</example>
    [Description("Nome da cidade")]
    public string City { get; set; } = string.Empty;
    
    /// <summary>
    /// Sigla do estado (UF)
    /// </summary>
    /// <example>SP</example>
    [Description("Sigla do estado")]
    public string State { get; set; } = string.Empty;
}
