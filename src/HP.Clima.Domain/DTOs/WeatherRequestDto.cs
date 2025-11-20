using System.ComponentModel;

namespace HP.Clima.Domain.DTOs;

/// <summary>
/// DTO para requisição de previsão do tempo
/// </summary>
/// <param name="Days">Número de dias para previsão (1-7)</param>
public record WeatherRequestDto(
    /// <summary>
    /// Quantidade de dias de previsão desejada (mínimo: 1, máximo: 7)
    /// </summary>
    /// <example>3</example>
    [Description("Número de dias de previsão (1-7)")]
    int Days = 3
);
