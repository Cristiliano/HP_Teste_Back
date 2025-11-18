using HP.Clima.Domain.Models;
using Refit;

namespace HP.Clima.Service.Proxies.BrasilApi;

public interface IBrasilApiProxy
{
    [Get("/api/cep/v2/{cep}")]
    Task<IApiResponse<BrasilApiCepResponse>> GetCepWithResponseAsync(string cep);
}
