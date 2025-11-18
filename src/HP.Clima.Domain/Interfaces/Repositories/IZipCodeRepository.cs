using HP.Clima.Domain.Entities;

namespace HP.Clima.Domain.Repositories;

public interface IZipCodeRepository
{
    Task<ZipCodeEntity?> GetByZipCodeAsync(string zipCode);
    Task<IEnumerable<ZipCodeEntity>> GetAllAsync();
    Task CreateAsync(ZipCodeEntity zipCodeEntity);
    Task UpdateAsync(ZipCodeEntity zipCodeEntity);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(string zipCode);
}
