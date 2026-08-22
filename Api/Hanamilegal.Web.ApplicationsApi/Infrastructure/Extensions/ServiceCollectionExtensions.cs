using System;
using System.Reflection;
using AutoMapper;
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

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection SetupMapper(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        static void Configure(IMapperConfigurationExpression configuration)
        {
            configuration.AddProfile(new ApplicationProfile());
        }

        var mappingConfig = new MapperConfiguration(Configure, null);

        IMapper mapper = mappingConfig.CreateMapper();

        return services
            .AddAutoMapper(t => { })
            .AddSingleton(mapper);
    }

    internal static IServiceCollection SetupProviders(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        return services.AddScoped<IApiInfoProvider, ApiInfoProvider>();
    }

    internal static IServiceCollection SetupServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        return services.AddScoped<IApplicationsService, ApplicationsService>();
    }

    internal static IServiceCollection SetupRepositories(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        return services.AddScoped<IApplicationsRepository, ApplicationsRepository>();
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
                string? executingAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;

                options.UseNpgsql(
                    connectionString,
                    t => t.MigrationsAssembly(executingAssemblyName));
            });
    }
}
