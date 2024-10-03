using MongoDB.Bson;

namespace Edc.Identity.Infrastructure.MongoDb.Dal;

public interface IMongoDocument
{
    ObjectId Id { get; set; }
}