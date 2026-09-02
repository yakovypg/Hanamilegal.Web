using System;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.AccountsApi.Configuration;
using Hanamilegal.Web.AccountsApi.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hanamilegal.Web.AccountsApi.Services;

public sealed class RefreshTokenCleanupService : BackgroundService
{
    private readonly RefreshTokenCleanupOptions _options;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RefreshTokenCleanupService> _logger;

    public RefreshTokenCleanupService(
        IOptions<RefreshTokenCleanupOptions> options,
        IServiceScopeFactory scopeFactory,
        ILogger<RefreshTokenCleanupService> logger)
    {
        ArgumentNullException.ThrowIfNull(options, nameof(options));
        ArgumentNullException.ThrowIfNull(scopeFactory, nameof(scopeFactory));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _options = options.Value;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_options.Interval);

        await CleanupAsync(stoppingToken);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await CleanupAsync(stoppingToken);
        }
    }

    private async Task CleanupAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cleaning up refresh tokens");

        try
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            IRefreshTokenRepository refreshTokenRepository = scope.ServiceProvider
                .GetRequiredService<IRefreshTokenRepository>();

            DateTimeOffset revokedBeforeUtc = DateTimeOffset.UtcNow
                .Subtract(_options.RevokedTokenRetention);

            await refreshTokenRepository.DeleteObsoleteAsync(revokedBeforeUtc, cancellationToken);

            _logger.LogInformation("Refresh token cleanup completed");
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to clean up refresh tokens");
        }
    }
}
