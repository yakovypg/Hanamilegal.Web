using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Hanamilegal.Web.AccountsApi.Infrastructure.Data.Db;

public sealed class AccountsDbContextFactory
    : IDesignTimeDbContextFactory<AccountsDbContext>
{
    public AccountsDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ACCOUNTS_DB_CONNECTION_STRING")
            ?? throw new InvalidOperationException("Connection string not found");

        var optionsBuilder = new DbContextOptionsBuilder<AccountsDbContext>();
        string? assemblyName = typeof(AccountsDbContext).Assembly.GetName().Name;

        optionsBuilder.UseNpgsql(
            connectionString,
            t => t.MigrationsAssembly(assemblyName));

        return new AccountsDbContext(optionsBuilder.Options);
    }
}
