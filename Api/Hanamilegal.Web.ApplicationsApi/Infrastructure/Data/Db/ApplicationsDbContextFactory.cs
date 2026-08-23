using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Db;

public sealed class ApplicationsDbContextFactory
    : IDesignTimeDbContextFactory<ApplicationsDbContext>
{
    public ApplicationsDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("APPLICATIONS_DB_CONNECTION_STRING")
            ?? throw new InvalidOperationException("Connection string not found");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationsDbContext>();
        string? assemblyName = typeof(ApplicationsDbContext).Assembly.GetName().Name;

        optionsBuilder.UseNpgsql(
            connectionString,
            t => t.MigrationsAssembly(assemblyName));

        return new ApplicationsDbContext(optionsBuilder.Options);
    }
}
