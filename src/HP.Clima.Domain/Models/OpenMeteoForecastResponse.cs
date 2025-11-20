using System.Text.Json.Serialization;

namespace HP.Clima.Domain.Models;

public class OpenMeteoForecastResponse
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }
    
    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }
    
    [JsonPropertyName("generationtime_ms")]
    public double GenerationtimeMs { get; set; }
    
    [JsonPropertyName("utc_offset_seconds")]
    public int UtcOffsetSeconds { get; set; }
    
    [JsonPropertyName("timezone")]
    public string Timezone { get; set; } = string.Empty;
    
    [JsonPropertyName("timezone_abbreviation")]
    public string TimezoneAbbreviation { get; set; } = string.Empty;
    
    [JsonPropertyName("elevation")]
    public double Elevation { get; set; }
    
    [JsonPropertyName("current_units")]
    public CurrentUnits CurrentUnits { get; set; } = new();
    
    [JsonPropertyName("current")]
    public Current Current { get; set; } = new();
    
    [JsonPropertyName("daily_units")]
    public DailyUnits DailyUnits { get; set; } = new();
    
    [JsonPropertyName("daily")]
    public Daily Daily { get; set; } = new();
}

public class CurrentUnits
{
    [JsonPropertyName("time")]
    public string Time { get; set; } = string.Empty;
    
    [JsonPropertyName("interval")]
    public string Interval { get; set; } = string.Empty;
    
    [JsonPropertyName("temperature_2m")]
    public string Temperature2m { get; set; } = string.Empty;
    
    [JsonPropertyName("relative_humidity_2m")]
    public string RelativeHumidity2m { get; set; } = string.Empty;
    
    [JsonPropertyName("apparent_temperature")]
    public string ApparentTemperature { get; set; } = string.Empty;
}

public class Current
{
    [JsonPropertyName("time")]
    public string Time { get; set; } = string.Empty;
    
    [JsonPropertyName("interval")]
    public int Interval { get; set; }
    
    [JsonPropertyName("temperature_2m")]
    public double Temperature2m { get; set; }
    
    [JsonPropertyName("relative_humidity_2m")]
    public int RelativeHumidity2m { get; set; }
    
    [JsonPropertyName("apparent_temperature")]
    public double ApparentTemperature { get; set; }
}

public class DailyUnits
{
    [JsonPropertyName("time")]
    public string Time { get; set; } = string.Empty;
    
    [JsonPropertyName("temperature_2m_max")]
    public string Temperature2mMax { get; set; } = string.Empty;
    
    [JsonPropertyName("temperature_2m_min")]
    public string Temperature2mMin { get; set; } = string.Empty;
}

public class Daily
{
    [JsonPropertyName("time")]
    public List<string> Time { get; set; } = new();
    
    [JsonPropertyName("temperature_2m_max")]
    public List<double> Temperature2mMax { get; set; } = new();
    
    [JsonPropertyName("temperature_2m_min")]
    public List<double> Temperature2mMin { get; set; } = new();
}
