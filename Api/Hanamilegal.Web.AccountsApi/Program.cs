using Hanamilegal.Web.AccountsApi.Infrastructure.Extensions;
using Hanamilegal.Web.ApiConfiguration.Extensions;
using Microsoft.AspNetCore.Builder;

WebApplicationBuilder appBuilder = WebApplication.CreateBuilder(args);

_ = appBuilder.Configuration.AddAppSettingsJson(
    basePath: appBuilder.Environment.ContentRootPath,
    environmentName: appBuilder.Environment.EnvironmentName);

_ = appBuilder.Services
    .SetupForwardedHeaders()
    .SetupStandardApiServices()
    .SetupOptions()
    .SetupCors(appBuilder.Configuration)
    .SetupAuthentication(appBuilder.Configuration)
    .SetupAuthorization()
    .SetupProviders()
    .SetupServices()
    .SetupRepositories()
    .SetupAccountsDb(appBuilder.Configuration)
    .SetupSwagger(appBuilder.Configuration)
    .SetupApiExceptionHandler();

WebApplication app = appBuilder.Build();

_ = app
    .SetupApiExceptionHandler()
    .SetupForwardedHeaders()
    .SetupStandardApiMiddlewares()
    .SetupSwagger();

_ = app.MapControllers();

app.Run();
