using Hanamilegal.Web.AccountsApi.Infrastructure.Extensions;
using Hanamilegal.Web.ApiConfiguration.Extensions;
using Microsoft.AspNetCore.Builder;

WebApplicationBuilder appBuilder = WebApplication.CreateBuilder(args);

_ = appBuilder.Configuration.AddAppSettingsJson(
    basePath: appBuilder.Environment.ContentRootPath,
    environmentName: appBuilder.Environment.EnvironmentName);

_ = appBuilder.Services
    .SetupStandardApiServices()
    .SetupAuthentication(appBuilder.Configuration)
    .SetupAuthorization()
    .SetupApiExceptionHandler()
    .SetupSwagger(appBuilder.Configuration)
    .SetupOptions()
    .SetupProviders()
    .SetupServices()
    .SetupRepositories()
    .SetupAccountsDb(appBuilder.Configuration);

WebApplication app = appBuilder.Build();

_ = app
    .SetupStandardApiMiddlewares()
    .SetupApiExceptionHandler()
    .SetupSwaggerApp();

app.Run();
