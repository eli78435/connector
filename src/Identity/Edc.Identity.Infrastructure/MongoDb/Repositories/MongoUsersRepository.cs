using Edc.Identity.Accessors;
using Edc.Identity.Infrastructure.MongoDb.Dal;
using Edc.Identity.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Edc.Identity.Infrastructure.MongoDb.Repositories;

public class MongoUsersRepository : IUsersRepository
{
    private readonly ILogger _logger;
    private readonly IMongoCollectionHolder<UserDal> _usersCollectionHolder;

    public MongoUsersRepository(ILogger<MongoUsersRepository> logger, 
        IMongoCollectionHolder<UserDal> usersCollectionHolder)
    {
        _logger = logger;
        _usersCollectionHolder = usersCollectionHolder;
    }
    
    public async Task<string> AddUser(User user)
    {
        var userDal = new UserDal
        {
            Identifier = user.Identifier,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Password = user.Password,
            Role = user.Role
        };
        
        await _usersCollectionHolder.Collection.InsertOneAsync(userDal);
        _logger.LogInformation("Add new user {UserName}", userDal.UserName);
        return userDal.Identifier;
    }
    
    public async Task<User?> GetUserByUserName(string userName)
    {
        var userDal = await _usersCollectionHolder.Collection
            .Find(u => u.UserName == userName)
            .FirstOrDefaultAsync();

        if (userDal == null) return null;

        return Convert(userDal);
    }

    public async Task<User?> GetUserByEmailAndPassword(string email, string password)
    {
        var userDal = await _usersCollectionHolder.Collection
            .Find(u => u.Email == email && u.Password == password)
            .FirstOrDefaultAsync();

        if (userDal == null) return null;

        return Convert(userDal);
    }

    private User Convert(UserDal userDal)
    {
        return new User(userDal.Identifier,
            userDal.FirstName,
            userDal.LastName,
            userDal.UserName,
            userDal.Email,
            userDal.Password,
            userDal.Role);
    }
}