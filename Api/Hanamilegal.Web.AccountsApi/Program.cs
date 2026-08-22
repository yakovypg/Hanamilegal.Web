using System.IO;
using Hanamilegal.Web.AccountsApi.Infrastructure.Extensions;
using Hanamilegal.Web.ApiConfiguration.Extensions;
using Microsoft.AspNetCore.Builder;

WebApplicationBuilder appBuilder = WebApplication.CreateBuilder(args);

_ = appBuilder.Configuration.AddAppSettingsJson(
    basePath: Directory.GetCurrentDirectory(),
    environmentName: appBuilder.Environment.EnvironmentName);

_ = appBuilder.Services
    .SetupStandardServices()
    .SetupAuthentication(appBuilder.Configuration)
    .SetupAuthorization()
    .SetupExceptionHandler()
    .SetupSwagger(appBuilder.Configuration)
    .SetupProviders()
    .SetupServices()
    .SetupRepositories()
    .SetupAccountsDb(appBuilder.Configuration);

WebApplication app = appBuilder.Build();

_ = app
    .SetupStandardMiddlewares()
    .SetupExceptionHandler()
    .SetupSwaggerApp();

app.Run();
