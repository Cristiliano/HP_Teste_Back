namespace HP.Clima.Domain.Configuration;

public class HttpClientOptions
{
    public const string SectionName = "HttpClients";

    public int TimeoutSeconds { get; set; } = 30;
    public ApiEndpointOptions BrasilApi { get; set; } = new();
    public ApiEndpointOptions ViaCep { get; set; } = new();
    public ApiEndpointOptions OpenMeteo { get; set; } = new();
    public ApiEndpointOptions OpenMeteoGeocoding { get; set; } = new();
    public ApiEndpointOptions OpenWeatherMap { get; set; } = new();
}

public class ApiEndpointOptions
{
    public string BaseUrl { get; set; } = string.Empty;
}
