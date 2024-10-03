using Edc.Identity.Models;

namespace Edc.Identity.Utilities;

public interface IUserTokenGenerator
{
    ValueTask<string?> Generate(User user);
}