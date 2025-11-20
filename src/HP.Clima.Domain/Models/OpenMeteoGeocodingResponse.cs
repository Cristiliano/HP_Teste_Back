namespace HP.Clima.Domain.Models;

public class OpenMeteoGeocodingResponse
{
    public List<GeocodingResult> Results { get; set; } = new();
    public double GenerationtimeMs { get; set; }
}

public class GeocodingResult
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Elevation { get; set; }
    public string? FeatureCode { get; set; }
    public string CountryCode { get; set; } = string.Empty;
    public int? Admin1Id { get; set; }
    public int? Admin2Id { get; set; }
    public int? Admin3Id { get; set; }
    public int? Admin4Id { get; set; }
    public string? Timezone { get; set; }
    public int? Population { get; set; }
    public List<string>? Postcodes { get; set; }
    public int CountryId { get; set; }
    public string Country { get; set; } = string.Empty;
    public string? Admin1 { get; set; }
    public string? Admin2 { get; set; }
    public string? Admin3 { get; set; }
    public string? Admin4 { get; set; }
}
