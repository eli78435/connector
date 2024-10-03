using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Edc.Identity.Models;
using Edc.Identity.Utilities;
using Microsoft.IdentityModel.Tokens;

namespace Edc.Identity.WebApi.Utilities;

internal class JwtUserTokenGenerator : IUserTokenGenerator
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtUserTokenGenerator(string secretKey, string issuer, string audience)
    {
        _secretKey = secretKey;
        _issuer = issuer;
        _audience = audience;
    }

    public ValueTask<string?> Generate(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        var hashedToken = new JwtSecurityTokenHandler().WriteToken(token);
        return new ValueTask<string?>(hashedToken);
    }
}