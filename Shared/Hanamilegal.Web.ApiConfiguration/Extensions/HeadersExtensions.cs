using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

namespace Hanamilegal.Web.ApiConfiguration.Extensions;

public static class HeadersExtensions
{
    public static IServiceCollection SetupForwardedHeaders(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        return services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto;
        });
    }

    public static IApplicationBuilder SetupForwardedHeaders(this IApplicationBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder, nameof(appBuilder));
        return appBuilder.UseForwardedHeaders();
    }
}
