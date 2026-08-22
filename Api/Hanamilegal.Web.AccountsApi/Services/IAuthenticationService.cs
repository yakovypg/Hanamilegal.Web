using System.Threading.Tasks;
using Hanamilegal.Web.Auth.Models;
using Hanamilegal.Web.Contracts.Accounts;

namespace Hanamilegal.Web.AccountsApi.Services;

internal interface IAuthenticationService
{
    Task<AuthenticationResult> AuthenticateAsync(LoginRequestDto dto);
}
