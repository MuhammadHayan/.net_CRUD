using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoCrudApi.Models;

namespace MongoCrudApi.Services;

public class UserService
{
    private readonly IMongoCollection<User> _usersCollection;

    public UserService(IOptions<MongoDbSettings> mongoSettings)
    {
        var mongoClient = new MongoClient(mongoSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
        _usersCollection = mongoDatabase.GetCollection<User>(mongoSettings.Value.CollectionName);
    }

    // CRUD Methods
    public async Task<List<User>> GetAsync() => await _usersCollection.Find(_ => true).ToListAsync();
    
    public async Task<User?> GetByIdAsync(string id) => 
        await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(User newUser) => await _usersCollection.InsertOneAsync(newUser);

    public async Task UpdateAsync(string id, User updatedUser) => 
        await _usersCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);

    public async Task RemoveAsync(string id) => await _usersCollection.DeleteOneAsync(x => x.Id == id);
}