namespace HP.Clima.Infra.Configuration;

public class MongoDbSettings
{
    public string ConnectionString { get; init; } = string.Empty;
    public string DatabaseName { get; init; } = string.Empty;
    public string ZipCodeLookupCollectionName { get; init; } = "ZipCodeLookups";
}
