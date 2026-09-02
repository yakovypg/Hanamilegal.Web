using Hanamilegal.Web.Auth.Models;
using Microsoft.IdentityModel.Tokens;

namespace Hanamilegal.Web.Auth.Services;

public interface IAccessTokenService
{
    AccessToken CreateAccessToken(AuthenticationResult authenticationResult);
    TokenValidationParameters CreateTokenValidationParameters();
}
