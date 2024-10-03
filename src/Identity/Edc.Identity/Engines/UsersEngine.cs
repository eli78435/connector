using Edc.Identity.Accessors;
using Edc.Identity.Models;
using Edc.Identity.Utilities;
using Microsoft.Extensions.Logging;

namespace Edc.Identity.Engines;

public interface IUsersEngine
{
    Task<User?> GetUserByUserNameAndPassword(string userName, string password);
    Task<string> AddUser(User newUser);
}

internal class UsersEngine : IUsersEngine
{
    private readonly ILogger _logger;
    private readonly IUsersRepository _usersRepository;
    private readonly IUserNameAndPasswordHashValidator _userNameAndPasswordHashValidator;

    public UsersEngine(ILogger<UsersEngine> logger,
        IUsersRepository usersRepository,
        IUserNameAndPasswordHashValidator userNameAndPasswordHashValidator)
    {
        _logger = logger;
        _usersRepository = usersRepository;
        _userNameAndPasswordHashValidator = userNameAndPasswordHashValidator;
    }
    
    public async Task<User?> GetUserByUserNameAndPassword(string userName, string password)
    {
        var existingUser = await _usersRepository.GetUserByUserName(userName);
        if (existingUser == null)
        {
            return null;
        }
        
        var valid = _userNameAndPasswordHashValidator.Validate(userName, password, existingUser.Password);
        if (valid)
        {
            return existingUser;
        }

        return null;
    }

    public Task<string> AddUser(User newUser)
    {
        var hashPassword = _userNameAndPasswordHashValidator.Hash(newUser.UserName, newUser.Password);
        var userWithHashPassword = newUser with { Password = hashPassword };
        return _usersRepository.AddUser(userWithHashPassword);
    }
}