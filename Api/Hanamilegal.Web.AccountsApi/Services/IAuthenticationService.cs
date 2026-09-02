using System.Threading.Tasks;
using Hanamilegal.Web.Auth.Models;
using Hanamilegal.Web.Contracts.Accounts;

namespace Hanamilegal.Web.AccountsApi.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResult> AuthenticateAsync(LoginRequestDto dto);
    Task<AuthenticationResult> GetAuthenticationResultAsync(string userId);
}
