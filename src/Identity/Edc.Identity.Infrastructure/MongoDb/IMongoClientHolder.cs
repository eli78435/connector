using MongoDB.Driver;

namespace Edc.Identity.Infrastructure.MongoDb;

public interface IMongoClientHolder
{
    MongoClient Client { get; }
}