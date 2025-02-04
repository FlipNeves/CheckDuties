using CheckDuties.Domain.Entities.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CheckDuties.Domain.Entities;

public class User : BaseEntity
{
    [BsonElement("username")]
    public string Username { get; set; }

    [BsonElement("password_hash")]
    public string PasswordHash { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("roles")]
    public List<string> Roles { get; set; } = new List<string>();

    public User(string username, string passwordHash)
    {
        Username = username;
        PasswordHash = passwordHash;
    }

    public void AddRole(string role)
    {
        if (!Roles.Contains(role))
            Roles.Add(role);
    }
}

