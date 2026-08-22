using System;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.AccountsApi.Infrastructure.Data.Db;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hanamilegal.Web.AccountsApi.Services;

internal sealed class AccountsDbStartupInitializerHostedService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;

    internal AccountsDbStartupInitializerHostedService(IServiceScopeFactory scopeFactory)
    {
        ArgumentNullException.ThrowIfNull(scopeFactory, nameof(scopeFactory));
        _scopeFactory = scopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IAccountsDbInitializer initializer = scope.ServiceProvider.GetRequiredService<IAccountsDbInitializer>();

        await initializer.InitializeDatabaseAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
