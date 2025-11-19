using System.Text.Json.Serialization;

namespace HP.Clima.Domain.Models;

public class BrasilApiCepResponse
{
    [JsonPropertyName("cep")]
    public string Cep { get; set; } = string.Empty;

    [JsonPropertyName("state")]
    public string State { get; set; } = string.Empty;

    [JsonPropertyName("city")]
    public string City { get; set; } = string.Empty;

    [JsonPropertyName("neighborhood")]
    public string Neighborhood { get; set; } = string.Empty;

    [JsonPropertyName("street")]
    public string Street { get; set; } = string.Empty;

    [JsonPropertyName("location")]
    public LocationData? Location { get; set; }
}

public class LocationData
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("coordinates")]
    public CoordinatesData? Coordinates { get; set; }
}

public class CoordinatesData
{
    [JsonPropertyName("longitude")]
    public string Longitude { get; set; } = string.Empty;

    [JsonPropertyName("latitude")]
    public string Latitude { get; set; } = string.Empty;
}
