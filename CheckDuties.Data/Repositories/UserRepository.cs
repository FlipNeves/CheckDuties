using CheckDuties.Data.Repositories.Base;
using CheckDuties.Domain.Entities;
using CheckDuties.Domain.Interfaces.Repositories;
using MongoDB.Driver;
using static CheckDuties.Data.ConstantesInfra;

namespace CheckDuties.Data.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(IMongoClient mongoClient)
        : base(mongoClient, Entidades.User, Collections.CheckDuties)
    {
    }
}
