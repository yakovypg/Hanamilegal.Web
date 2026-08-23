using Hanamilegal.Web.AccountsApi.Infrastructure.Extensions;
using Hanamilegal.Web.ApiConfiguration.Extensions;
using Hanamilegal.Web.ApiConfiguration.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

WebApplicationBuilder appBuilder = WebApplication.CreateBuilder(args);

_ = appBuilder.Configuration.AddAppSettingsJson(
    basePath: appBuilder.Environment.ContentRootPath,
    environmentName: appBuilder.Environment.EnvironmentName);

_ = appBuilder.Services
    .SetupStandardApiServices()
    .SetupOptions()
    .SetupAuthentication(appBuilder.Configuration)
    .SetupAuthorization()
    .SetupProviders()
    .SetupServices()
    .SetupRepositories()
    .SetupAccountsDb(appBuilder.Configuration)
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
