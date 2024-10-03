namespace Edc.Identity.Models;

public record User(string Identifier,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Password,
    string Role);