using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.Auth.Services;

namespace Hanamilegal.Web.InternalApp.Services;

public sealed class TokenRefreshCoordinator : ITokenRefreshCoordinator
{
    private readonly IRefreshTokenHasher _refreshTokenHasher;
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks;

    public TokenRefreshCoordinator(IRefreshTokenHasher refreshTokenHasher)
    {
        ArgumentNullException.ThrowIfNull(refreshTokenHasher, nameof(refreshTokenHasher));

        _locks = new();
        _refreshTokenHasher = refreshTokenHasher;
    }

    public string CreateLockKey(string refreshToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken, nameof(refreshToken));
        return _refreshTokenHasher.HashRefreshToken(refreshToken);
    }

    public async Task<T?> RunAsync<T>(
        string lockKey,
        Func<CancellationToken, Task<T?>> refresh,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lockKey, nameof(lockKey));
        ArgumentNullException.ThrowIfNull(refresh, nameof(refresh));

        SemaphoreSlim semaphore = _locks.GetOrAdd(lockKey, static _ => new SemaphoreSlim(1, 1));

        await semaphore.WaitAsync(cancellationToken);

        try
        {
            return await refresh(cancellationToken);
        }
        finally
        {
            semaphore.Release();

            var lockToRemove = new KeyValuePair<string, SemaphoreSlim>(lockKey, semaphore);
            _ = _locks.TryRemove(lockToRemove);
        }
    }
}
