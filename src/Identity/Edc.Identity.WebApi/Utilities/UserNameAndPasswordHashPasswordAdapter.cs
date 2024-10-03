using Edc.Identity.Utilities;
using Microsoft.AspNetCore.Identity;

namespace Edc.Identity.WebApi.Utilities;

internal class UserNameAndPasswordHashPasswordAdapter : IUserNameAndPasswordHashValidator
{
    private readonly PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();

    public string Hash(string userName, string password)
    {
        return _passwordHasher.HashPassword(userName, password);
    }

    public bool Validate(string userName, string password, string hash)
    {
        var validationResult = _passwordHasher.VerifyHashedPassword(userName, hash, password);
        return validationResult == PasswordVerificationResult.Success;
    }
}