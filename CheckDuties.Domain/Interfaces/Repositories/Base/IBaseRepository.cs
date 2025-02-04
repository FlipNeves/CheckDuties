using MongoDB.Bson;

namespace CheckDuties.Domain.Interfaces.Repositories.Base;

public interface IBaseRepository<T>
{
    Task AddAsync(T entity);
    Task<T> GetByIdAsync(ObjectId id);
    Task<List<T>> GetAllAsync();
    Task UpdateAsync(T entity);
    Task DeleteAsync(ObjectId id);
}
