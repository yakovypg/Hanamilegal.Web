using System;
using Hanamilegal.Web.ApiConfiguration.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Hanamilegal.Web.ApiConfiguration.Extensions;

public static class ExceptionHandlerExtensions
{
    public static IServiceCollection SetupApiExceptionHandler(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        return services.AddScoped<ApiExceptionHandler>();
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
