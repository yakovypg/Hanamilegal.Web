using System;
using Asp.Versioning;
using Hanamilegal.Web.ApiConfiguration.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
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
}
