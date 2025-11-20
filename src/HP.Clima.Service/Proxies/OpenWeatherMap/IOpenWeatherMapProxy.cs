using HP.Clima.Domain.Models;
using Refit;

namespace HP.Clima.Service.Proxies.OpenWeatherMap;

public interface IOpenWeatherMapProxy
{
    [Get("/data/2.5/weather")]
    Task<IApiResponse<OpenWeatherMapCurrentResponse>> GetCurrentWeatherAsync(
        [AliasAs("lat")] double latitude,
        [AliasAs("lon")] double longitude,
        [AliasAs("appid")] string apiKey,
        [AliasAs("units")] string units = "metric");

    [Get("/data/2.5/forecast")]
    Task<IApiResponse<OpenWeatherMapForecastResponse>> GetForecastAsync(
        [AliasAs("lat")] double latitude,
        [AliasAs("lon")] double longitude,
        [AliasAs("appid")] string apiKey,
        [AliasAs("units")] string units = "metric");
}
