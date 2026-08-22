using System;
using Hanamilegal.Web.ApiConfiguration.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Hanamilegal.Web.ApiConfiguration.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder SetupStandardMiddlewares(this IApplicationBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder, nameof(appBuilder));

        return appBuilder
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseHttpsRedirection()
            .UseEndpoints(t => t.MapControllers());
    }

    public static IApplicationBuilder SetupExceptionHandler(this IApplicationBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder, nameof(appBuilder));

        var apiExceptionHandler = appBuilder.ApplicationServices.GetRequiredService<ApiExceptionHandler>();

        var exceptionHandlerOptions = new ExceptionHandlerOptions()
        {
            AllowStatusCode404Response = true,
            ExceptionHandler = async httpContext => await apiExceptionHandler.HandleExceptionAsync(httpContext)
        };

        return appBuilder.UseExceptionHandler(exceptionHandlerOptions);
    }
}
