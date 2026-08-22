using System.Threading.Tasks;
using Hanamilegal.Web.AccountsApi.Contracts;

namespace Hanamilegal.Web.AccountsApi.Services;

public interface IAccountsService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
}
