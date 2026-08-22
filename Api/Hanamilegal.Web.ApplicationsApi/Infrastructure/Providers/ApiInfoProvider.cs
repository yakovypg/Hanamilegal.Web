using System;
using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Hanamilegal.Web.ApiConfiguration.Providers;
using Microsoft.OpenApi.Models;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Providers;

internal class ApiInfoProvider : IApiInfoProvider
{
    public OpenApiInfo CreateApiInfo(ApiVersionDescription versionDescription)
    {
        ArgumentNullException.ThrowIfNull(versionDescription, nameof(versionDescription));
        
        string title = Assembly.GetExecutingAssembly().GetName().Name ?? "API";

        var apiInfo = new OpenApiInfo()
        {
            Title = title,
            Version = versionDescription.GroupName,
            Description = string.Empty
        };

        if (versionDescription.IsDeprecated)
            apiInfo.Description += "This API version is deprecated and will be removed in the future.";

        return apiInfo;
    }
}
