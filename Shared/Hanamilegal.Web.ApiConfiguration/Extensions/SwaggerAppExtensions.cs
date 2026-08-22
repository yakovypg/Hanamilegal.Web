using System;
using System.Collections.Generic;
using System.Linq;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Hanamilegal.Web.ApiConfiguration.Extensions;

public static class SwaggerAppExtensions
{
    public static IApplicationBuilder SetupSwaggerApp(this IApplicationBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder, nameof(appBuilder));

        IApiVersionDescriptionProvider apiVersionDescriptionProvider =
            appBuilder.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

        return appBuilder.UseSwagger().UseSwaggerUI(options =>
        {
            const string swaggerRoutePrefix = "api/doc";

            IEnumerable<ApiVersionDescription> apiVersionDescriptions = apiVersionDescriptionProvider
                .ApiVersionDescriptions
                .OrderByDescending(e => e.ApiVersion.MajorVersion)
                .ThenByDescending(e => e.ApiVersion.MinorVersion);

            foreach (ApiVersionDescription description in apiVersionDescriptions)
            {
                string groupName = description.GroupName;

                options.SwaggerEndpoint(
                    $"/{swaggerRoutePrefix}/{groupName}/swagger.json",
                    groupName.ToUpperInvariant()
                );
            }

            options.RoutePrefix = swaggerRoutePrefix;
        });
    }
}
