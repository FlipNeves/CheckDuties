using CheckDuties.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace CheckDuties.Data.Context;

public class MongoDbContext
{
    public IMongoDatabase Db { get; set; }

    public MongoDbContext(IConfiguration configuration)
    {
        try
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(configuration["ConnectionString"]));
            var client = new MongoClient(settings);
            Db = client.GetDatabase(configuration["DbName"]);
            MapClasses();
        }
        catch (Exception ex) { throw new MongoException("It was not possible to just connect to MongoDb", ex); }
    }

    private void MapClasses()
    {
        var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("camelCase", conventionPack, t => true);

        if (!BsonClassMap.IsClassMapRegistered(typeof(Duty)))
        {
            BsonClassMap.RegisterClassMap<Duty>(i =>
            {
                i.AutoMap();
                i.SetIgnoreExtraElements(true);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
        {
            BsonClassMap.RegisterClassMap<User>(i =>
            {
                i.AutoMap();
                i.SetIgnoreExtraElements(true);
            });
        }
    }
}
