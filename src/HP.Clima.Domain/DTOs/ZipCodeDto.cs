namespace HP.Clima.Domain.DTOs;

public class ZipCodeDto
{
    public string ZipCode { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Ibge { get; set; } = string.Empty;
    public LocationDto Location { get; set; } = new();
    public string Provider { get; set; } = string.Empty;
}

public class LocationDto
{
    public double Lat { get; set; }
    public double Lon { get; set; }
}
