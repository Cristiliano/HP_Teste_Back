namespace HP.Clima.Domain.DTOs;

public class CepRequestDto
{
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
