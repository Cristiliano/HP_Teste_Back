namespace HP.Clima.Domain.DTOs;

public class LocationDto
{
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
}
