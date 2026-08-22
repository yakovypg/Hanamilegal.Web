using System;
using System.Text.Json.Serialization;
using Hanamilegal.Web.Auth.Authorization;
using Hanamilegal.Web.Auth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hanamilegal.Web.ApiConfiguration.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection SetupStandardServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        _ = services
            .AddHttpClient()
            .AddMemoryCache()
            .AddRouting(options => options.LowercaseUrls = true);

        _ = services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

        return services;
    }

    public static IServiceCollection SetupAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        AuthenticationBuilder builder = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        var jwtTokenService = new JwtTokenService(configuration);

        _ = builder.AddJwtBearer(options =>
        {
            options.TokenValidationParameters = jwtTokenService.CreateTokenValidationParameters();
        });

        return services;
    }

    public static IServiceCollection SetupAuthorization(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        _ = services.AddSingleton<IAuthorizationHandler, RoleAtLeastHandler>();

        return services.AddAuthorization(options =>
        {
            options.AddPolicy(
                nameof(RoleAtLeastRequirement.RoleAtLeastUser),
                policy => policy.Requirements.Add(RoleAtLeastRequirement.RoleAtLeastUser));

            options.AddPolicy(
                nameof(RoleAtLeastRequirement.RoleAtLeastApplicationViewer),
                policy => policy.Requirements.Add(RoleAtLeastRequirement.RoleAtLeastApplicationViewer));

            options.AddPolicy(
                nameof(RoleAtLeastRequirement.RoleAtLeastAdmin),
                policy => policy.Requirements.Add(RoleAtLeastRequirement.RoleAtLeastAdmin));
        });
    }
}
