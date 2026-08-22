using System.IO;
using Hanamilegal.Web.ApiConfiguration.Extensions;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;

WebApplicationBuilder appBuilder = WebApplication.CreateBuilder(args);

_ = appBuilder.Configuration.AddAppSettingsJson(
    basePath: Directory.GetCurrentDirectory(),
    environmentName: appBuilder.Environment.EnvironmentName);

_ = appBuilder.Services
    .SetupStandardApiServices()
    .SetupAuthentication(appBuilder.Configuration)
    .SetupAuthorization()
    .SetupApiExceptionHandler()
    .SetupSwagger(appBuilder.Configuration)
    .SetupMapper()
    .SetupProviders()
    .SetupServices()
    .SetupRepositories()
    .SetupApplicationsDb(appBuilder.Configuration);

WebApplication app = appBuilder.Build();

_ = app
    .SetupStandardApiMiddlewares()
    .SetupApiExceptionHandler()
    .SetupSwaggerApp();

app.Run();
