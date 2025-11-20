namespace HP.Clima.Infra.Configuration;

public class MongoDbSettings
{
    public const string SectionName = "MongoDb";
    
    public bool UseInMemory { get; init; }
    public string ConnectionString { get; init; } = string.Empty;
    public string DatabaseName { get; init; } = "HP_Clima_DB";
    public string ZipCodeLookupCollectionName { get; init; } = "ZipCodeLookups";
}
