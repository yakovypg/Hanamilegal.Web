using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hanamilegal.Web.InternalApp.Services;

public interface ITokenRefreshCoordinator
{
    string CreateLockKey(string refreshToken);

    Task<T?> RunAsync<T>(
        string lockKey,
        Func<CancellationToken, Task<T?>> refresh,
        CancellationToken cancellationToken = default);
}
