using System;
using Hanamilegal.Web.ApiConfiguration.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

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

    public static IApplicationBuilder SetupApiExceptionHandler(this IApplicationBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder, nameof(appBuilder));

        return appBuilder.UseExceptionHandler(app =>
        {
            app.Run(async context =>
            {
                ApiExceptionHandler exceptionhandler =
                    context.RequestServices.GetRequiredService<ApiExceptionHandler>();

                await exceptionhandler.HandleExceptionAsync(context);
            });
        });
    }
}
