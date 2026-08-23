using System;
using Hanamilegal.Web.ApiConfiguration.Handlers;
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

    public static IApplicationBuilder SetupApiExceptionHandler(
        this IApplicationBuilder appBuilder,
        ApiExceptionHandler apiExceptionHandler)
    {
        ArgumentNullException.ThrowIfNull(appBuilder, nameof(appBuilder));
        ArgumentNullException.ThrowIfNull(apiExceptionHandler, nameof(apiExceptionHandler));

        var exceptionHandlerOptions = new ExceptionHandlerOptions()
        {
            AllowStatusCode404Response = true,
            ExceptionHandler = async httpContext => await apiExceptionHandler.HandleExceptionAsync(httpContext)
        };

        return appBuilder.UseExceptionHandler(exceptionHandlerOptions);
    }
}
