using System.Threading.Tasks;
using Hanamilegal.Web.Contracts.Accounts;

namespace Hanamilegal.Web.AccountsApi.Services;

public interface IAuthenticationService
{
    Task<(bool Ok, string UserId)> AuthenticateAsync(LoginRequestDto dto);
}
