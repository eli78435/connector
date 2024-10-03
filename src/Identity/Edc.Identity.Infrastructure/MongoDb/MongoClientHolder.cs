using MongoDB.Driver;

namespace Edc.Identity.Infrastructure.MongoDb;

public class MongoClientHolder : IMongoClientHolder
{
    public MongoClientHolder(string connectionString)
    {
        Client = new MongoClient(connectionString);
    }
    
    public MongoClient Client { get; }
}