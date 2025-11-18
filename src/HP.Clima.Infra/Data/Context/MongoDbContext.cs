using HP.Clima.Domain.Entities;
using HP.Clima.Infra.Configuration;
using MongoDB.Driver;

namespace HP.Clima.Infra.Data.Context;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.DatabaseName);
    }

    public IMongoCollection<ZipCodeEntity> ZipCodeLookups => 
        _database.GetCollection<ZipCodeEntity>("ZipCodeLookups");
}
