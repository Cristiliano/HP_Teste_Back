namespace HP.Clima.API.DTOs;

public class CepRequestDto
{
    public string ZipCode { get; set; } = string.Empty;
    
    public string GetNormalizedZipCode()
    {
        return ZipCode.Replace("-", "").PadLeft(8, '0');
    }
}