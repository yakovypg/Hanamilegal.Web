using System;
using System.Collections.Generic;
using System.Linq;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Hanamilegal.Web.ApiConfiguration.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Hanamilegal.Web.ApiConfiguration.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection SetupSwagger(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        _ = services
            .AddOptions<SwaggerOptions>()
            .BindConfiguration(SwaggerOptions.SectionName)
            .Validate(o => !string.IsNullOrWhiteSpace(o.ApiVersion))
            .ValidateOnStart();

        SwaggerOptions swaggerOptions = configuration
            .GetRequiredObject<SwaggerOptions>(SwaggerOptions.SectionName);

        var apiVersion = new Version(swaggerOptions.ApiVersion);
        string schemeName = JwtBearerDefaults.AuthenticationScheme;

        IApiVersioningBuilder apiVersioningBuilder = services
            .AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");
                options.DefaultApiVersion = new ApiVersion(apiVersion.Major, apiVersion.Minor);
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        _ = services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        return services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(t => t.FullName);

            options.AddSecurityDefinition(schemeName, new OpenApiSecurityScheme()
            {
                Scheme = schemeName,
                Name = "Authorization",
                BearerFormat = "JWT",
                Description = $"Enter **{schemeName} {{TOKEN}}** to access this API",
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement()
            {
                [new OpenApiSecuritySchemeReference(schemeName, document)] = []
            });

            /*
            string? executingAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            string baseDirectory = Directory.GetCurrentDirectory();
            string assemblyXmlFile = $"{executingAssemblyName}.xml";
            string assemblyXmlPath = Path.Combine(baseDirectory, assemblyXmlFile);

            options.IncludeXmlComments(assemblyXmlPath);
            */
        });
    }

    public static IApplicationBuilder SetupSwagger(this IApplicationBuilder appBuilder)
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
                    groupName.ToUpperInvariant());
            }

            options.RoutePrefix = swaggerRoutePrefix;
        });
    }
}
