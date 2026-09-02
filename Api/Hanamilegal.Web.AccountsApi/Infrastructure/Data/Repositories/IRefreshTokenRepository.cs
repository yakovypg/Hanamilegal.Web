using System.Threading.Tasks;
using Hanamilegal.Web.AccountsApi.Domain.Entities;

namespace Hanamilegal.Web.AccountsApi.Infrastructure.Data.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);
    Task ReplaceAsync(RefreshToken oldRefreshToken, RefreshToken newRefreshToken);
    Task<RefreshToken?> FindByHashAsync(string refreshTokenHash);
}
