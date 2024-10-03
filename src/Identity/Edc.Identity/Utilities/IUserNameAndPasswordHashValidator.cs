namespace Edc.Identity.Utilities;

public interface IUserNameAndPasswordHashValidator
{
    string Hash(string userName, string password);
    bool Validate(string userName, string password, string hash);
}