using System;
using Microsoft.AspNetCore.Builder;

namespace Hanamilegal.Web.InternalApp.Infrastructure.Extensions;

internal static class ApplicationBuilderExtensions
{
    internal static IApplicationBuilder SetupStandardMiddlewares(this IApplicationBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder, nameof(appBuilder));

        return appBuilder
            .UseHttpsRedirection()
            .UseStaticFiles()
            .UseRouting()
            .UseAuthorization();
    }

    internal static IApplicationBuilder SetupExceptionHandler(
        this IApplicationBuilder appBuilder,
        bool isDevelopment)
    {
        ArgumentNullException.ThrowIfNull(appBuilder, nameof(appBuilder));

        if (!isDevelopment)
        {
            appBuilder
                .UseExceptionHandler("/Error")
                .UseHsts();
        }
        else
        {
            // appBuilder.UseDeveloperExceptionPage();
            appBuilder.UseExceptionHandler("/Error");
        }

        return appBuilder;
    }
}
