using HP.Clima.Domain.DTOs;

namespace HP.Clima.Service.Handlers.Interfaces;

public interface ICepApiHandler
{
    string ApiName { get; }
    Task<(bool Success, ZipCodeDto? Data)> TryGetCepAsync(string normalizedZipCode);
}
