using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.AccountsApi.Domain.Entities;
using Hanamilegal.Web.AccountsApi.Infrastructure.Data.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hanamilegal.Web.AccountsApi.Infrastructure.Data.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AccountsDbContext _context;
    private readonly ILogger<RefreshTokenRepository> _logger;

    public RefreshTokenRepository(AccountsDbContext context, ILogger<RefreshTokenRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _context = context;
        _logger = logger;
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        ArgumentNullException.ThrowIfNull(refreshToken, nameof(refreshToken));

        _logger.LogInformation("Trying to add refresh token");

        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Refresh token added");
    }

    public async Task ReplaceAsync(RefreshToken oldRefreshToken, RefreshToken newRefreshToken)
    {
        ArgumentNullException.ThrowIfNull(oldRefreshToken, nameof(oldRefreshToken));
        ArgumentNullException.ThrowIfNull(newRefreshToken, nameof(newRefreshToken));

        _logger.LogInformation(
            "Trying to replace refresh token {OldRefreshTokenId}",
            oldRefreshToken.Id);

        oldRefreshToken.RevokedAtUtc = DateTimeOffset.UtcNow;
        oldRefreshToken.ReplacedByTokenHash = newRefreshToken.TokenHash;

        await AddAsync(newRefreshToken);

        _logger.LogInformation(
            "Refresh token {OldRefreshTokenId} replaced",
            oldRefreshToken.Id);
    }

    public async Task<RefreshToken?> FindByHashAsync(string refreshTokenHash)
    {
        ArgumentException.ThrowIfNullOrEmpty(refreshTokenHash, nameof(refreshTokenHash));

        _logger.LogInformation("Trying to find refresh token by hash");

        RefreshToken? foundRefreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == refreshTokenHash);

        _logger.LogInformation("Refresh token found: {Found}", foundRefreshToken is not null);

        return foundRefreshToken;
    }

    public async Task DeleteObsoleteAsync(
        DateTimeOffset revokedBeforeUtc,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Trying to delete obsolete refresh tokens");

        await _context.RefreshTokens
            .Where(t =>
                t.ExpiresAtUtc <= DateTimeOffset.UtcNow ||
                t.RevokedAtUtc <= revokedBeforeUtc)
            .ExecuteDeleteAsync(cancellationToken);

        _logger.LogInformation("Obsolete refresh tokens deleted");
    }
}
