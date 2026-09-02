using System.Threading.Tasks;
using Hanamilegal.Web.Contracts.Accounts;

namespace Hanamilegal.Web.AccountsApi.Services;

public interface IAccountsService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    Task<LoginResponseDto> RefreshAccessTokenAsync(RefreshAccessTokenRequestDto dto);
}
