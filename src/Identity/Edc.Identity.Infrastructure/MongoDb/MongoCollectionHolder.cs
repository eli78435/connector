using MongoDB.Driver;

namespace Edc.Identity.Infrastructure.MongoDb;

internal class MongoCollectionHolder<TDocument> : IMongoCollectionHolder<TDocument>
{
    public string DatabaseName { get; }
    public string CollectionName { get; }
    
    public IMongoClient MongoClient { get; }
    public IMongoDatabase Database { get; }
    public IMongoCollection<TDocument> Collection { get; }

    public MongoCollectionHolder(IMongoClient mongoClient, 
        string databaseName, 
        string collectionName,
        bool generateCollectionIfNotExist = true)
    {
        DatabaseName = databaseName;
        CollectionName = collectionName;

        MongoClient = mongoClient;
        Database = MongoClient.GetDatabase(databaseName);
        Collection = Database.GetCollection<TDocument>(CollectionName);

        if (generateCollectionIfNotExist && Collection == null)
        {
            Database.CreateCollection(collectionName);
            Collection = Database.GetCollection<TDocument>(CollectionName);
        }
    }
}