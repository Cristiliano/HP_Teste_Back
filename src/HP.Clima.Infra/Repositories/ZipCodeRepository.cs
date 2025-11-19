using HP.Clima.Domain.Entities;
using HP.Clima.Domain.Repositories;
using HP.Clima.Infra.Data.Context;
using MongoDB.Driver;

namespace HP.Clima.Infra.Repositories;

public class ZipCodeRepository(MongoDbContext context) : IZipCodeRepository
{
    private readonly IMongoCollection<ZipCodeEntity> _collection = context.ZipCodeLookups;

    public async Task<ZipCodeEntity?> GetByZipCodeAsync(string zipCode)
    {
        return await _collection
            .Find(x => x.ZipCode == zipCode)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ZipCodeEntity>> GetAllAsync()
    {
        return await _collection
            .Find(_ => true)
            .ToListAsync();
    }

    public async Task CreateAsync(ZipCodeEntity zipCodeEntity)
    {
        zipCodeEntity.CreatedAtUtc = DateTime.UtcNow;
        await _collection.InsertOneAsync(zipCodeEntity);
    }

    public async Task UpdateAsync(ZipCodeEntity zipCodeEntity)
    {
        await _collection.ReplaceOneAsync(
            x => x.Id == zipCodeEntity.Id, 
            zipCodeEntity);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _collection.DeleteOneAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsAsync(string zipCode)
    {
        var count = await _collection
            .CountDocumentsAsync(x => x.ZipCode == zipCode);
        return count > 0;
    }
}
