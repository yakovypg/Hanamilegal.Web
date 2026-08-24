using System;
using Hanamilegal.Web.ApiConfiguration.Options;
using Microsoft.AspNetCore.Builder;

namespace Hanamilegal.Web.ApiConfiguration.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder SetupStandardApiMiddlewares(this IApplicationBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder, nameof(appBuilder));

        return appBuilder
            .UseHttpsRedirection()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization();
    }

    public static IApplicationBuilder SetupCors(this IApplicationBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder, nameof(appBuilder));
        return appBuilder.UseCors(nameof(CorsOptions.Frontend));
    }
}
