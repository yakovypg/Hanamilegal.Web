using System.Security.Claims;
using Hanamilegal.Web.Auth.Models;
using Microsoft.IdentityModel.Tokens;

namespace Hanamilegal.Web.Auth.Services;

public interface ITokenService
{
    TokenValidationParameters CreateTokenValidationParameters();
    AccessToken CreateAccessToken(string userId, params Claim[] extraClaims);
}
