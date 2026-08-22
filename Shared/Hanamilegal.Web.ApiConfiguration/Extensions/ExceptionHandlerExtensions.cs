using System;
using Hanamilegal.Web.ApiConfiguration.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Hanamilegal.Web.ApiConfiguration.Extensions;

public static class ExceptionHandlerExtensions
{
    public static IServiceCollection SetupApiExceptionHandler(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        return services.AddScoped<ApiExceptionHandler>();
    }
}
