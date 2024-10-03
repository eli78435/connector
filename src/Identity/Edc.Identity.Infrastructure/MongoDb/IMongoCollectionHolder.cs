using MongoDB.Driver;

namespace Edc.Identity.Infrastructure.MongoDb;

public interface IMongoCollectionHolder<TDocument>
{
    string CollectionName { get; }
    IMongoCollection<TDocument> Collection { get; }
}