namespace Edc.Identity.WebApi.ViewModels;

public class UserViewModel
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    private string UserName { get; set; }
    string Email { get; set; }
    string Password { get; set; }
    string Role { get; set; }
}