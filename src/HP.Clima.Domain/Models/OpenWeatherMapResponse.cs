namespace HP.Clima.Domain.Models;

public class OpenWeatherMapCurrentResponse
{
    public Coord Coord { get; set; } = new();
    public List<Weather> Weather { get; set; } = new();
    public string Base { get; set; } = string.Empty;
    public Main Main { get; set; } = new();
    public int Visibility { get; set; }
    public Wind Wind { get; set; } = new();
    public Clouds Clouds { get; set; } = new();
    public long Dt { get; set; }
    public Sys Sys { get; set; } = new();
    public int Timezone { get; set; }
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Cod { get; set; }
}

public class OpenWeatherMapForecastResponse
{
    public string Cod { get; set; } = string.Empty;
    public int Message { get; set; }
    public int Cnt { get; set; }
    public List<ForecastItem> List { get; set; } = new();
    public City City { get; set; } = new();
}

public class ForecastItem
{
    public long Dt { get; set; }
    public Main Main { get; set; } = new();
    public List<Weather> Weather { get; set; } = new();
    public Clouds Clouds { get; set; } = new();
    public Wind Wind { get; set; } = new();
    public int Visibility { get; set; }
    public double Pop { get; set; }
    public Sys Sys { get; set; } = new();
    public string DtTxt { get; set; } = string.Empty;
}

public class Main
{
    public double Temp { get; set; }
    public double FeelsLike { get; set; }
    public double TempMin { get; set; }
    public double TempMax { get; set; }
    public int Pressure { get; set; }
    public int Humidity { get; set; }
    public int? SeaLevel { get; set; }
    public int? GrndLevel { get; set; }
}

public class Wind
{
    public double Speed { get; set; }
    public int Deg { get; set; }
    public double? Gust { get; set; }
}

public class Clouds
{
    public int All { get; set; }
}

public class Sys
{
    public int? Type { get; set; }
    public int? Id { get; set; }
    public string? Country { get; set; }
    public long? Sunrise { get; set; }
    public long? Sunset { get; set; }
    public string? Pod { get; set; }
}

public class Weather
{
    public int Id { get; set; }
    public string Main { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}

public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Coord Coord { get; set; } = new();
    public string Country { get; set; } = string.Empty;
    public int Population { get; set; }
    public int Timezone { get; set; }
    public long Sunrise { get; set; }
    public long Sunset { get; set; }
}

public class Coord
{
    public double Lon { get; set; }
    public double Lat { get; set; }
}
