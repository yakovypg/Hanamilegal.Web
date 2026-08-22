using System;
using Asp.Versioning;
using Hanamilegal.Web.ApiConfiguration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Hanamilegal.Web.ApiConfiguration.Extensions;

public static class SwaggerServiceExtensions
{
    public static IServiceCollection SetupSwagger(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        string apiVersionString = configuration.GetString("Swagger:ApiVersion");
        string schemeName = configuration.GetString("Swagger:SchemeName");

        var apiVersion = new Version(apiVersionString);

        IApiVersioningBuilder apiVersioningBuilder = services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = new HeaderApiVersionReader("api-version");
            options.DefaultApiVersion = new ApiVersion(apiVersion.Major, apiVersion.Minor);
        });

        _ = apiVersioningBuilder.AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        _ = services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

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

            var securityRequirementScheme = new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Id = schemeName,
                    Type = ReferenceType.SecurityScheme
                }
            };

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                { securityRequirementScheme, Array.Empty<string>() }
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
}
