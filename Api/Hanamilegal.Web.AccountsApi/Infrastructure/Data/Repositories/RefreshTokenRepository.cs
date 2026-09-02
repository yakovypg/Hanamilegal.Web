using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.AccountsApi.Domain.Entities;
using Hanamilegal.Web.AccountsApi.Infrastructure.Data.Db;
using Microsoft.EntityFrameworkCore;

namespace Hanamilegal.Web.AccountsApi.Infrastructure.Data.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AccountsDbContext _context;

    public RefreshTokenRepository(AccountsDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        _context = context;
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        ArgumentNullException.ThrowIfNull(refreshToken, nameof(refreshToken));

        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task ReplaceAsync(RefreshToken oldRefreshToken, RefreshToken newRefreshToken)
    {
        ArgumentNullException.ThrowIfNull(oldRefreshToken, nameof(oldRefreshToken));
        ArgumentNullException.ThrowIfNull(newRefreshToken, nameof(newRefreshToken));

        oldRefreshToken.RevokedAtUtc = DateTimeOffset.UtcNow;
        oldRefreshToken.ReplacedByTokenHash = newRefreshToken.TokenHash;

        await AddAsync(newRefreshToken);
    }

    public async Task<RefreshToken?> FindByHashAsync(string refreshTokenHash)
    {
        ArgumentException.ThrowIfNullOrEmpty(refreshTokenHash, nameof(refreshTokenHash));

        return await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == refreshTokenHash);
    }

    public async Task DeleteObsoleteAsync(
        DateTimeOffset revokedBeforeUtc,
        CancellationToken cancellationToken = default)
    {
        await _context.RefreshTokens
            .Where(t =>
                t.ExpiresAtUtc <= DateTimeOffset.UtcNow ||
                t.RevokedAtUtc <= revokedBeforeUtc)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
