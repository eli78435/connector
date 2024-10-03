using Edc.Common;
using MongoDB.Bson;

namespace Edc.Identity.Infrastructure.MongoDb;

internal class MongoIdGenerator : IIdGenerator
{
    public string GenerateId()
    {
        return ObjectId.GenerateNewId().ToString();
    }
}