using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FreeStuff.Identity.Api.Domain;
using FreeStuff.Identity.Api.Domain.Ports;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FreeStuff.Identity.Api.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateJwt(User user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Sid, user.Id),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: credentials
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        return jwt;
    }
}
