using Hanamilegal.Web.Auth.Models;
using Microsoft.IdentityModel.Tokens;

namespace Hanamilegal.Web.Auth.Services;

public interface ITokenService
{
    AccessToken CreateAccessToken(AuthenticationResult authenticationResult);
    TokenValidationParameters CreateTokenValidationParameters();
}
