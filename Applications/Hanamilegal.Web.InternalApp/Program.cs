using Hanamilegal.Web.ApiConfiguration.Extensions;
using Hanamilegal.Web.InternalApp.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

WebApplicationBuilder appBuilder = WebApplication.CreateBuilder(args);

_ = appBuilder.Configuration.AddAppSettingsJson(
    basePath: appBuilder.Environment.ContentRootPath,
    environmentName: appBuilder.Environment.EnvironmentName);

_ = appBuilder.Services
    .SetupHeaders()
    .SetupStandardServices()
    .SetupOptions()
    .SetupServices()
    .SetupAuthenticationWithCookies()
    .SetupAuthorization()
    .SetupHttpClients()
    .AddPersistentKeyStorage(appBuilder.Configuration);

WebApplication app = appBuilder.Build();

_ = app
    .SetupExceptionHandler(app.Environment.IsDevelopment())
    .SetupHeaders()
    .SetupStandardMiddlewares();

_ = app.MapRazorPages();

app.Run();
