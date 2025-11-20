using System.ComponentModel;

namespace HP.Clima.Domain.DTOs;

/// <summary>
/// DTO de resposta com informações completas do CEP
/// </summary>
public class ZipCodeDto
{
    /// <summary>
    /// CEP normalizado (apenas números, 8 dígitos)
    /// </summary>
    /// <example>01311000</example>
    [Description("CEP normalizado sem hífen")]
    public string ZipCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Nome da rua/avenida/logradouro
    /// </summary>
    /// <example>Avenida Paulista</example>
    [Description("Nome do logradouro")]
    public string Street { get; set; } = string.Empty;
    
    /// <summary>
    /// Nome do bairro/distrito
    /// </summary>
    /// <example>Bela Vista</example>
    [Description("Nome do bairro")]
    public string District { get; set; } = string.Empty;
    
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
    [Description("Sigla do estado (UF)")]
    public string State { get; set; } = string.Empty;
    
    /// <summary>
    /// Código IBGE do município
    /// </summary>
    /// <example>3550308</example>
    [Description("Código IBGE")]
    public string Ibge { get; set; } = string.Empty;
    
    /// <summary>
    /// Coordenadas geográficas (latitude e longitude)
    /// </summary>
    [Description("Localização geográfica")]
    public LocationZipCodeDto Location { get; set; } = new();
    
    /// <summary>
    /// Provedor da API que retornou os dados (brasilapi ou viacep)
    /// </summary>
    /// <example>brasilapi</example>
    [Description("Provedor de dados")]
    public string Provider { get; set; } = string.Empty;
}

/// <summary>
/// Coordenadas geográficas do CEP
/// </summary>
public class LocationZipCodeDto
{
    /// <summary>
    /// Latitude em graus decimais
    /// </summary>
    /// <example>-23.5613</example>
    [Description("Latitude")]
    public double Lat { get; set; }
    
    /// <summary>
    /// Longitude em graus decimais
    /// </summary>
    /// <example>-46.6565</example>
    [Description("Longitude")]
    public double Lon { get; set; }
}
