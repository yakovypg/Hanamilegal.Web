using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hanamilegal.Web.Auth.Models;
using Hanamilegal.Web.Auth.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Hanamilegal.Web.Auth.Services;

public class JwtTokenService : IAccessTokenService
{
    public const int MinJwtKeyBits = 32 * 8;
    public const string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;

    private readonly IOptions<JwtOptions> _options;

    public JwtTokenService(IOptions<JwtOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options, nameof(options));
        _options = options;
    }

    public AccessToken CreateAccessToken(AuthenticationResult authenticationResult)
    {
        DateTime expireDateUtc = DateTime.UtcNow.AddMinutes(_options.Value.ExpireMinutes);
        SigningCredentials signingCredentials = CreateSigningCredentials(_options.Value.Key);
        IEnumerable<Claim> claims = CreateClaims(authenticationResult);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _options.Value.Issuer,
            audience: _options.Value.Audience,
            claims: claims,
            expires: expireDateUtc,
            signingCredentials: signingCredentials);

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        string token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

        return new AccessToken(token, expireDateUtc);
    }

    public TokenValidationParameters CreateTokenValidationParameters()
    {
        SymmetricSecurityKey signingKey = CreateSymmetricSecurityKey(_options.Value.Key);

        return new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,

            ValidateIssuer = true,
            ValidIssuer = _options.Value.Issuer,

            ValidateAudience = true,
            ValidAudience = _options.Value.Audience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(_options.Value.ClockSkewSeconds),

            ValidAlgorithms = [SecurityAlgorithm],
            RequireExpirationTime = true
        };
    }

    private static SymmetricSecurityKey CreateSymmetricSecurityKey(string key)
    {
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));

        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        const int minJwtKeyBytes = MinJwtKeyBits / 8;

        if (keyBytes.Length < minJwtKeyBytes)
            throw new InvalidOperationException($"JWT key must contain at least {MinJwtKeyBits} bits");

        return new SymmetricSecurityKey(keyBytes);
    }

    private static SigningCredentials CreateSigningCredentials(string key)
    {
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));

        SymmetricSecurityKey signingKey = CreateSymmetricSecurityKey(key);
        return new SigningCredentials(signingKey, SecurityAlgorithm);
    }

    private static List<Claim> CreateClaims(AuthenticationResult authenticationResult)
    {
        string jti = Guid.NewGuid().ToString();

        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Jti, jti),
            new(ClaimTypes.NameIdentifier, authenticationResult.UserId)
        ];

        if (!string.IsNullOrEmpty(authenticationResult.UserEmail))
        {
            var emailClaim = new Claim(ClaimTypes.Email, authenticationResult.UserEmail);
            claims.Add(emailClaim);
        }

        foreach (string role in authenticationResult.UserRoles)
        {
            var roleClaim = new Claim(ClaimTypes.Role, role);
            claims.Add(roleClaim);
        }

        return claims;
    }
}
