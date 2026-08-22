using System.Threading.Tasks;
using Hanamilegal.Web.Contracts.Accounts;

namespace Hanamilegal.Web.AccountsApi.Services;

internal interface IAuthenticationService
{
    Task<(bool Ok, string UserId)> AuthenticateAsync(LoginRequestDto dto);
}
