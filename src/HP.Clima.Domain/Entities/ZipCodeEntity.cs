using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HP.Clima.Domain.Entities;

public class ZipCodeEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [BsonElement("zipCode")]
    public string ZipCode { get; set; } = string.Empty;

    [BsonElement("street")]
    public string Street { get; set; } = string.Empty;

    [BsonElement("district")]
    public string District { get; set; } = string.Empty;

    [BsonElement("city")]
    public string City { get; set; } = string.Empty;

    [BsonElement("state")]
    public string State { get; set; } = string.Empty;

    [BsonElement("ibge")]
    public string Ibge { get; set; } = string.Empty;

    [BsonElement("lat")]
    public double? Lat { get; set; }

    [BsonElement("lon")]
    public double? Lon { get; set; }

    [BsonElement("provider")]
    public string Provider { get; set; } = string.Empty;

    [BsonElement("createdAtUtc")]
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
