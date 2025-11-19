using HP.Clima.Domain.DTOs;

namespace HP.Clima.Service.Handlers;

public interface ICepApiHandler
{
    string ApiName { get; }
    Task<(bool Success, ZipCodeDto? Data)> TryGetCepAsync(string normalizedZipCode);
}
