using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shopzy.Application.Abstractions.Interfaces;
using Shopzy.Application.Common;
using Shopzy.Application.Configurations;
using Shopzy.Application.Dtos;
using Shopzy.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Shopzy.Infrastructure.Jwt;

public sealed class JwtGenerator : IJwtGenerator
{
    private readonly int _tokenLifespanInHours = 1;
    private readonly JwtConfiguration _jwtConfiguration;

    public JwtGenerator(IOptions<JwtConfiguration> options)
    {
        _jwtConfiguration = options.Value;
    }

    public JwtDto Generate(User User, IEnumerable<Role> userRoles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Convert.FromBase64String(_jwtConfiguration.Key);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, User.Username),
            new Claim(JwtRegisteredClaimNames.Email, User.Email)
        };

        foreach (var role in userRoles)
        {
            if (role is not null)
            {
                claims.Add(new Claim(CustomJwtRegisteredClaimNames.AssignedRoles, role.Name));
            }
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddHours(_tokenLifespanInHours),
            Issuer = _jwtConfiguration.Issuer,
            Audience = _jwtConfiguration.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        return new(jwt, _tokenLifespanInHours);
    }
}
