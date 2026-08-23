using Hanamilegal.Web.ApiConfiguration.Extensions;
using Hanamilegal.Web.ApiConfiguration.Handlers;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

WebApplicationBuilder appBuilder = WebApplication.CreateBuilder(args);

_ = appBuilder.Configuration.AddAppSettingsJson(
    basePath: appBuilder.Environment.ContentRootPath,
    environmentName: appBuilder.Environment.EnvironmentName);

_ = appBuilder.Services
    .SetupStandardApiServices()
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
ApiExceptionHandler apiExceptionHandler = app.Services.GetRequiredService<ApiExceptionHandler>();

_ = app
    .SetupApiExceptionHandler(apiExceptionHandler)
    .SetupStandardApiMiddlewares()
    .SetupSwaggerApp();

_ = app.MapControllers();

app.Run();
