using CheckDuties.Data.Repositories.Base;
using CheckDuties.Domain.Entities;
using CheckDuties.Domain.Interfaces.Repositories;
using MongoDB.Driver;
using static CheckDuties.Data.ConstantesInfra;

namespace CheckDuties.Data.Repositories;

public class DutyRepository : BaseRepository<Duty>, IDutyRepository
{
    public DutyRepository(IMongoClient mongoClient) 
        : base(mongoClient, Entidades.Duty, Collections.CheckDuties)
    {
    }
}
