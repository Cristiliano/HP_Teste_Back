using HP.Clima.Domain.Models;
using Refit;

namespace HP.Clima.Service.Proxies.OpenMeteo;

public interface IOpenMeteoForecastProxy
{
    [Get("/v1/forecast")]
    Task<IApiResponse<OpenMeteoForecastResponse>> GetForecastAsync(
        [AliasAs("latitude")] double latitude,
        [AliasAs("longitude")] double longitude,
        [AliasAs("current")] string current,
        [AliasAs("daily")] string daily,
        [AliasAs("forecast_days")] int forecastDays,
        [AliasAs("timezone")] string timezone = "auto");
}
