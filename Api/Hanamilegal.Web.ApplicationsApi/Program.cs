using Hanamilegal.Web.ApiConfiguration.Extensions;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;

WebApplicationBuilder appBuilder = WebApplication.CreateBuilder(args);

_ = appBuilder.Configuration.AddAppSettingsJson(
    basePath: appBuilder.Environment.ContentRootPath,
    environmentName: appBuilder.Environment.EnvironmentName);

_ = appBuilder.Services
    .SetupStandardApiServices()
    .SetupCors(appBuilder.Configuration)
    .SetupAuthentication(appBuilder.Configuration)
    .SetupAuthorization()
    .SetupProviders()
    .SetupServices()
    .SetupRepositories()
    .SetupApplicationsDb(appBuilder.Configuration)
    .SetupMapper()
    .SetupSwagger(appBuilder.Configuration)
    .SetupApiExceptionHandler();

WebApplication app = appBuilder.Build();

_ = app
    .SetupApiExceptionHandler()
    .SetupCors()
    .SetupStandardApiMiddlewares()
    .SetupSwagger();

_ = app.MapControllers();

app.Run();
