using Edc.Identity.Models;

namespace Edc.Identity.Accessors;

public interface IUsersRepository
{
    Task<User?> GetUserByUserName(string email);
    Task<string> AddUser(User user);
}