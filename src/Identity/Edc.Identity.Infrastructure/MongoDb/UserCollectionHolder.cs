using Edc.Identity.Infrastructure.MongoDb.Dal;
using MongoDB.Driver;

namespace Edc.Identity.Infrastructure.MongoDb;

internal class UserCollectionHolder : MongoCollectionHolder<UserDal>
{
    public UserCollectionHolder(IMongoClient mongoClient,
        string databaseName,
        string collectionName) : base(mongoClient, databaseName, collectionName)
    {
        var indexes = new[]
        {
            new CreateIndexModel<UserDal>(
                Builders<UserDal>.IndexKeys.Ascending(u => u.Identifier), 
                new CreateIndexOptions
                {
                    Unique = true
                }),
            new CreateIndexModel<UserDal>(
                Builders<UserDal>.IndexKeys.Ascending(u => u.UserName), 
                new CreateIndexOptions
                {
                    Unique = true
                }),
            new CreateIndexModel<UserDal>(
                Builders<UserDal>.IndexKeys.Ascending(u => u.Email), 
                new CreateIndexOptions
                {
                    Unique = true
                }),
        };

        Collection.Indexes.CreateMany(indexes);
    }
}