using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HP.Clima.Domain.DTOs;

/// <summary>
/// DTO para requisição de consulta/cadastro de CEP
/// </summary>
public class CepRequestDto
{
    /// <summary>
    /// CEP a ser consultado/cadastrado (com ou sem hífen)
    /// </summary>
    /// <example>01311-000</example>
    [Description("CEP brasileiro no formato XXXXX-XXX ou XXXXXXXX")]
    public string ZipCode { get; set; } = string.Empty;

    public CepRequestDto() { }

    public CepRequestDto(string zipCode)
    {
        ZipCode = zipCode;
    }

    public string GetNormalizedZipCode()
    {
        return ZipCode.Replace("-", "").PadLeft(8, '0');
    }
}
