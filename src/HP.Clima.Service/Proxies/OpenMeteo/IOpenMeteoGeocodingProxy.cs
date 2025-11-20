using HP.Clima.Domain.Models;
using Refit;

namespace HP.Clima.Service.Proxies.OpenMeteo;

public interface IOpenMeteoGeocodingProxy
{
    [Get("/v1/search")]
    Task<IApiResponse<OpenMeteoGeocodingResponse>> SearchLocationAsync(
        [AliasAs("name")] string name,
        [AliasAs("count")] int count = 1,
        [AliasAs("language")] string language = "pt",
        [AliasAs("format")] string format = "json");
}
