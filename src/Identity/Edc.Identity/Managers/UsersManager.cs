using Edc.Common;
using Edc.Identity.Engines;
using Edc.Identity.Models;
using Edc.Identity.Utilities;
using Microsoft.Extensions.Logging;

namespace Edc.Identity.Managers;

public interface IUsersManager
{
    Task<string> AddUser(User newUser);
    Task<string?> LoginByUserNameAndPassword(string userName, string password);
}

internal class UsersManager : IUsersManager
{
    private readonly ILogger _logger;
    private readonly IUsersEngine _usersEngine;
    private readonly IAuthenticationEngine _authenticationEngine;

    public UsersManager(ILogger<IUsersManager> logger,
        IUsersEngine usersEngine,
        IAuthenticationEngine authenticationEngine)
    {
        _logger = logger;
        _usersEngine = usersEngine;
        _authenticationEngine = authenticationEngine;
    }
    
    public async Task<string> AddUser(User newUser)
    {
        var userDetailsId = await _usersEngine.AddUser(newUser);
        return userDetailsId;
    }

    public async Task<string?> LoginByUserNameAndPassword(string userName, string password)
    {
        var user = await _usersEngine.GetUserByUserNameAndPassword(userName, password);
        if (user != null)
        {
            var token = await _authenticationEngine.GenerateToken(user);
            if (!string.IsNullOrWhiteSpace(token))
                return token;
        }

        return null;
    }
}