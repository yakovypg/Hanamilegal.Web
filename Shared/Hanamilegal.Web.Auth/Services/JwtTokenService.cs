using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Hanamilegal.Web.Auth.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Hanamilegal.Web.Auth.Services;

public class JwtTokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        _configuration = configuration;
    }

    public TokenValidationParameters CreateTokenValidationParameters()
    {
        string key = ExtractKeyFromConfiguration();
        string issuer = ExtractIssuerFromConfiguration();
        string audience = ExtractAudienceFromConfiguration();

        SymmetricSecurityKey signingKey = CreateSymmetricSecurityKey(key);

        return new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = signingKey
        };
    }

    public AccessToken CreateAccessToken(string userId, params Claim[] extraClaims)
    {
        ArgumentException.ThrowIfNullOrEmpty(userId, nameof(userId));
        ArgumentNullException.ThrowIfNull(extraClaims, nameof(extraClaims));

        string key = ExtractKeyFromConfiguration();
        string issuer = ExtractIssuerFromConfiguration();
        string audience = ExtractAudienceFromConfiguration();
        int expireMinutes = ExtractExpireMinutesFromConfiguration();

        DateTime expireDateUtc = DateTime.UtcNow.AddMinutes(expireMinutes);
        SigningCredentials signingCredentials = CreateSigningCredentials(key);

        IEnumerable<Claim> claims =
        [
            ..CreateUserClaims(userId),
            ..CreateRoleClaims(),
            ..extraClaims
        ];

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expireDateUtc,
            signingCredentials: signingCredentials);

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        string token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

        return new AccessToken(token, expireDateUtc);
    }

    private static SymmetricSecurityKey CreateSymmetricSecurityKey(string key)
    {
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));

        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        return new SymmetricSecurityKey(keyBytes);
    }

    private static SigningCredentials CreateSigningCredentials(string key)
    {
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));

        SymmetricSecurityKey signingKey = CreateSymmetricSecurityKey(key);
        return new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
    }

    private static List<Claim> CreateUserClaims(string userId)
    {
        ArgumentException.ThrowIfNullOrEmpty(userId, nameof(userId));

        string jti = Guid.NewGuid().ToString();

        return
        [
            new(JwtRegisteredClaimNames.Jti, jti),
            new(ClaimTypes.NameIdentifier, userId)
        ];
    }

    private static IEnumerable<Claim> CreateRoleClaims()
    {
        return Enum.GetNames<UserRole>()
            .Select(t => new Claim(ClaimTypes.Role, t));
    }

    private string ExtractIssuerFromConfiguration()
    {
        return _configuration["Jwt:Issuer"]
            ?? throw new KeyNotFoundException("Issuer for JWT not specified");
    }

    private string ExtractAudienceFromConfiguration()
    {
        return _configuration["Jwt:Audience"]
            ?? throw new KeyNotFoundException("Audience for JWT not specified");
    }

    private string ExtractKeyFromConfiguration()
    {
        return _configuration["Jwt:Key"]
            ?? throw new KeyNotFoundException("Key for JWT not specified");
    }

    private int ExtractExpireMinutesFromConfiguration()
    {
        string expireMinutesSource = _configuration["Jwt:ExpireMinutes"]
            ?? throw new KeyNotFoundException("Expire minutes for JWT not specified");

        if (!int.TryParse(expireMinutesSource, out int expireMinutes))
            throw new FormatException("Expire minutes for JWT not recognized");

        return expireMinutes;
    }
}
