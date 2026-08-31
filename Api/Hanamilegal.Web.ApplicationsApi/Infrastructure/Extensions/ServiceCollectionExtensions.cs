using System;
using Hanamilegal.Web.ApiConfiguration.Extensions;
using Hanamilegal.Web.ApiConfiguration.Providers;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Db;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Repositories;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Providers;
using Hanamilegal.Web.ApplicationsApi.Mapping.Profiles;
using Hanamilegal.Web.ApplicationsApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection SetupMapper(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        return services.AddAutoMapper(configuration =>
        {
            configuration.AddProfile<ApplicationProfile>();
            configuration.AddProfile<ConsentAuditProfile>();
        });
    }

    internal static IServiceCollection SetupProviders(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        return services.AddSingleton<IApiInfoProvider, ApiInfoProvider>();
    }

    internal static IServiceCollection SetupServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        return services
            .AddScoped<IApplicationsService, ApplicationsService>()
            .AddScoped<IConsentsService, ConsentsService>();
    }

    internal static IServiceCollection SetupRepositories(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        return services
            .AddScoped<IApplicationsRepository, ApplicationsRepository>()
            .AddScoped<IConsentsRepository, ConsentsRepository>();
    }

    internal static IServiceCollection SetupApplicationsDb(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        string connectionString = configuration.GetRequiredConnectionString("ApplicationsDb");

        return services
            .AddDbContext<ApplicationsDbContext>(options =>
            {
                string? executingAssemblyName = typeof(ApplicationsDbContext).Assembly.GetName().Name;

                options.UseNpgsql(
                    connectionString,
                    t => t.MigrationsAssembly(executingAssemblyName));
            });
    }

    internal static IServiceCollection SetupConsentsDb(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        const string consentsDbName = "ConsentsDb";
        string connectionString = configuration.GetRequiredConnectionString(consentsDbName);

        return services
            .AddSingleton<IMongoClient>(_ => new MongoClient(connectionString))
            .AddSingleton(t =>
            {
                return new ConsentsDbContext(
                    t.GetRequiredService<IMongoClient>(),
                    consentsDbName);
            });
    }
}
