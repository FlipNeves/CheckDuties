using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CheckDuties.Domain.Entities.Base;

public abstract class BaseEntity
{
    [BsonId]
    public ObjectId Id { get; set; }
}
