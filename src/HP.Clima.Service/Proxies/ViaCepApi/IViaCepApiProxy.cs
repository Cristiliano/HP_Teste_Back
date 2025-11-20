using HP.Clima.Domain.Models;
using Refit;

namespace HP.Clima.Service.Proxies.ViaCepApi;

public interface IViaCepApiProxy
{
    [Get("/ws/{cep}/json/")]
    Task<IApiResponse<ViaCepApiResponse>> GetCepWithResponseAsync([AliasAs("cep")] string cep);
}
