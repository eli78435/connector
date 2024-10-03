using MongoDB.Bson;

namespace Edc.Identity.Infrastructure.MongoDb.Dal;

public class UserDal : IMongoDocument
{
    public ObjectId Id { get; set; }
    public string Identifier { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}