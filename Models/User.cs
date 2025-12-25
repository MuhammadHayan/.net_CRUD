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

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    // 🔐 Stores HASHED password (never plain text)
    public string PasswordHash { get; set; } = null!;
}