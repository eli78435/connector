using Edc.Identity.Accessors;
using Edc.Identity.Models;
using Edc.Identity.Utilities;
using Microsoft.Extensions.Logging;

namespace Edc.Identity.Engines;

public interface IAuthenticationEngine
{
    ValueTask<string?> GenerateToken(User user);
}

internal class AuthenticationEngine : IAuthenticationEngine
{
    private readonly ILogger _logger;
    private readonly IUsersRepository _usersRepository;
    private readonly IUserTokenGenerator _tokenGenerator;

    public AuthenticationEngine(ILogger<AuthenticationEngine> logger,
        IUsersRepository usersRepository,
        IUserTokenGenerator tokenGenerator)
    {
        _logger = logger;
        _usersRepository = usersRepository;
        _tokenGenerator = tokenGenerator;
    }
    
    public ValueTask<string?> GenerateToken(User user)
    {
        return _tokenGenerator.Generate(user);
    }
}