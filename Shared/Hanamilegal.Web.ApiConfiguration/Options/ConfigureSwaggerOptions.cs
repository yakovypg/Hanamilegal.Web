using System;
using System.Collections.Generic;
using Asp.Versioning.ApiExplorer;
using Hanamilegal.Web.ApiConfiguration.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Hanamilegal.Web.ApiConfiguration.Options;

public sealed class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiInfoProvider _apiInfoProvider;
    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

    public ConfigureSwaggerOptions(
        IApiInfoProvider apiInfoProvider,
        IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
        ArgumentNullException.ThrowIfNull(apiInfoProvider, nameof(apiInfoProvider));
        ArgumentNullException.ThrowIfNull(apiVersionDescriptionProvider, nameof(apiVersionDescriptionProvider));

        _apiInfoProvider = apiInfoProvider;
        _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        ArgumentNullException.ThrowIfNull(options, nameof(options));

        IEnumerable<ApiVersionDescription> apiVersionDescriptions =
            _apiVersionDescriptionProvider.ApiVersionDescriptions;

        foreach (ApiVersionDescription description in apiVersionDescriptions)
        {
            OpenApiInfo apiInfo = _apiInfoProvider.CreateApiInfo(description);
            options.SwaggerDoc(description.GroupName, apiInfo);
        }
    }
}
