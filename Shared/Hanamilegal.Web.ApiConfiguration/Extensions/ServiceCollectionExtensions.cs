using System;
using System.Text.Json.Serialization;
using Hanamilegal.Web.ApiConfiguration.Options;
using Hanamilegal.Web.Auth.Authorization;
using Hanamilegal.Web.Auth.Options;
using Hanamilegal.Web.Auth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hanamilegal.Web.ApiConfiguration.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection SetupStandardApiServices(this IServiceCollection services)
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
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
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

        IOptions<JwtOptions> jwtOptions = configuration.GetRequiredOptions<JwtOptions>(JwtOptions.SectionName);
        JwtTokenService jwtTokenService = new(jwtOptions);

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

    public static IServiceCollection SetupCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        _ = services
            .AddOptions<CorsOptions>()
            .BindConfiguration(CorsOptions.SectionName)
            .Validate(o => o.Frontend?.AllowedOrigins is not null)
            .ValidateOnStart();

        CorsOptions options = configuration.GetRequiredObject<CorsOptions>(CorsOptions.SectionName);

        return services.AddCors(setup =>
        {
            setup.AddPolicy(nameof(options.Frontend), policy =>
            {
                policy
                    .WithOrigins([.. options.Frontend.AllowedOrigins])
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public static IServiceCollection AddJwtTokenService(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        _ = services
            .AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .Validate(o => o.ExpireMinutes > 0)
            .Validate(o => o.ClockSkewSeconds >= 0)
            .Validate(o => !string.IsNullOrWhiteSpace(o.Issuer))
            .Validate(o => !string.IsNullOrWhiteSpace(o.Audience))
            .ValidateOnStart();

        return services.AddScoped<ITokenService, JwtTokenService>();
    }
}
