using HP.Clima.Domain.DTOs;

namespace HP.Clima.Domain.Interfaces.Services;

public interface ICepService
{
    Task<ZipCodeDto?> GetCepInfoAsync(string zipCode);
    Task<ZipCodeDto> SaveCepInfoAsync(CepRequestDto cepRequest);
}
