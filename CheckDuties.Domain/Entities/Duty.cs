using CheckDuties.Domain.Entities.Base;
using CheckDuties.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CheckDuties.Domain.Entities;

public class Duty : BaseEntity
{

    [BsonElement("title")]
    public string? Title { get; set; }

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("created_date")]
    public DateTime CreatedDate { get; set; }

    [BsonElement("assigned_to")]
    public string? AssignedTo { get; set; }

    [BsonElement("status")]
    public StatusDuty Status { get; set; }

    [BsonElement("tags")]
    public List<string>? Tags { get; set; }

    [BsonElement("comments")]
    public List<string>? Comments { get; set; }

    [BsonElement("Constant")]
    public bool? Constant { get; set; }

    public void UpdateStatus(StatusDuty newStatus)
    {
        Status = newStatus;
    }

    public void AddTags(IEnumerable<string> newTags)
    {
        if (Tags == null) Tags = new List<string>();
        Tags.AddRange(newTags);
    }
}
