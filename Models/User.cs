using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoCrudApi.Models;

// This class represents how data is stored in MongoDB
// ❌ NEVER expose this directly to the client
public class User
{
    [BsonId] // Marks this as the Primary Key
    [BsonRepresentation(BsonType.ObjectId)] // Allows us to use 'string' instead of 'ObjectId'
    public string? Id { get; set; }

    [BsonElement("Name")] // Maps the C# property to a Mongo field name
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;
}