using CheckDuties.Domain.Entities.Base;
using CheckDuties.Domain.Interfaces.Repositories.Base;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CheckDuties.Data.Repositories.Base;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    private readonly IMongoCollection<T> _collection;

    public BaseRepository(IMongoClient mongoClient, string databaseName, string collectionName)
    {
        var database = mongoClient.GetDatabase(databaseName);
        _collection = database.GetCollection<T>(collectionName);
    }

    public async Task AddAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task<T> GetByIdAsync(ObjectId id)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _collection.Find(Builders<T>.Filter.Empty).ToListAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, entity.Id);
        await _collection.ReplaceOneAsync(filter, entity);
    }

    public async Task DeleteAsync(ObjectId id)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, id);
        await _collection.DeleteOneAsync(filter);
    }
}
