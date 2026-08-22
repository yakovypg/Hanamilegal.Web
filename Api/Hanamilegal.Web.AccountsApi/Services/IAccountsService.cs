using System.Threading.Tasks;
using Hanamilegal.Web.Contracts.Accounts;

namespace Hanamilegal.Web.AccountsApi.Services;

internal interface IAccountsService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
}
