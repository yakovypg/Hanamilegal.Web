using System;
using System.Linq;
using System.Reflection;
using Hanamilegal.Web.AccountsApi.Configuration;
using Hanamilegal.Web.AccountsApi.Infrastructure.Data.Db;
using Hanamilegal.Web.AccountsApi.Infrastructure.Data.Repositories;
using Hanamilegal.Web.AccountsApi.Infrastructure.Providers;
using Hanamilegal.Web.AccountsApi.Services;
using Hanamilegal.Web.ApiConfiguration.Extensions;
using Hanamilegal.Web.ApiConfiguration.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hanamilegal.Web.AccountsApi.Infrastructure.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection SetupOptions(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        _ = services
            .AddOptions<InitialUsersOptions>()
            .BindConfiguration(InitialUsersOptions.SectionName)
            .Validate(options =>
            {
                return options.Users.All(user =>
                    !string.IsNullOrWhiteSpace(user.Email) &&
                    !string.IsNullOrWhiteSpace(user.Password));
            })
            .ValidateOnStart();

        return services;
    }

    internal static IServiceCollection SetupProviders(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        return services.AddScoped<IApiInfoProvider, ApiInfoProvider>();
    }

    internal static IServiceCollection SetupServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        return services
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<IAccountsService, AccountsService>();
    }

    internal static IServiceCollection SetupRepositories(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        return services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IUserRoleRepository, UserRoleRepository>();
    }

    internal static IServiceCollection SetupAccountsDb(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        string connectionString = configuration.GetRequiredConnectionString("AccountsDb");

        _ = services
            .AddScoped<IAccountsDbInitializer, AccountsDbInitializer>()
            .AddHostedService<AccountsDbStartupInitializerHostedService>()
            .AddDbContext<AccountsDbContext>(options =>
            {
                string? executingAssemblyName = typeof(AccountsDbContext).Assembly.GetName().Name;

                options.UseNpgsql(
                    connectionString,
                    t => t.MigrationsAssembly(executingAssemblyName));
            });

        IdentityBuilder identityBuilder = services.AddIdentity<IdentityUser, IdentityRole>(setup =>
        {
            setup.User.RequireUniqueEmail = true;
            setup.DisablePasswordRequirements();
        });

        _ = identityBuilder
            .AddEntityFrameworkStores<AccountsDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}
