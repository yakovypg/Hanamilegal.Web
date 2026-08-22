using System;
using Hanamilegal.Web.ApiConfiguration.Extensions;
using Hanamilegal.Web.InternalApp.Api;
using Hanamilegal.Web.InternalApp.Api.Accounts;
using Hanamilegal.Web.InternalApp.Api.Applications;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hanamilegal.Web.InternalApp.Infrastructure.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection SetupStandardServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        _ = services.AddRazorPages();

        return services;
    }

    internal static IServiceCollection SetupAuthenticationWithCookies(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        _ = services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromDays(14);
                options.SlidingExpiration = true;
            });

        return services;
    }

    internal static IServiceCollection SetupOptions(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        _ = services
            .AddOptions<AccountsApiOptions>()
            .BindConfiguration(AccountsApiOptions.SectionName)
            .Validate(o => !string.IsNullOrWhiteSpace(o.BaseUrl))
            .ValidateOnStart();

        _ = services
            .AddOptions<ApplicationsApiOptions>()
            .BindConfiguration(ApplicationsApiOptions.SectionName)
            .Validate(o => !string.IsNullOrWhiteSpace(o.BaseUrl))
            .ValidateOnStart();

        return services;
    }

    internal static IServiceCollection SetupServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        return services
            .AddHttpContextAccessor()
            .AddTransient<ApiAuthorizationHandler>()
            .AddJwtTokenService();
    }

    internal static IServiceCollection SetupHttpClients(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        _ = services.AddHttpClient<AccountsApiClient>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<AccountsApiOptions>>();
            client.BaseAddress = new Uri(options.Value.BaseUrl);
        });

        _ = services.AddHttpClient<ApplicationsApiClient>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<ApplicationsApiOptions>>();
            client.BaseAddress = new Uri(options.Value.BaseUrl);
        }).AddHttpMessageHandler<ApiAuthorizationHandler>();

        return services;
    }
}
