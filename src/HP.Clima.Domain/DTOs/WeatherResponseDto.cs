namespace HP.Clima.Domain.DTOs;

public class WeatherResponseDto
{
    public int SourceZipCodeId { get; set; }
    public LocationDto Location { get; set; } = new();
    public CurrentWeatherDto Current { get; set; } = new();
    public List<DailyWeatherDto> Daily { get; set; } = new();
    public string Provider { get; set; } = string.Empty;
}

public class CurrentWeatherDto
{
    public double TemperatureC { get; set; }
    public double Humidity { get; set; }
    public double ApparentTemperatureC { get; set; }
    public DateTime ObservedAt { get; set; }
}

public class DailyWeatherDto
{
    public string Date { get; set; } = string.Empty;
    public double TempMinC { get; set; }
    public double TempMaxC { get; set; }
}
