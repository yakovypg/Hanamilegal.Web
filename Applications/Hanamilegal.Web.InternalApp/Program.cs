using System.IO;
using Hanamilegal.Web.ApiConfiguration.Extensions;
using Hanamilegal.Web.InternalApp.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

WebApplicationBuilder appBuilder = WebApplication.CreateBuilder(args);

_ = appBuilder.Configuration.AddAppSettingsJson(
    basePath: Directory.GetCurrentDirectory(),
    environmentName: appBuilder.Environment.EnvironmentName);

_ = appBuilder.Services
    .SetupStandardServices()
    .SetupAuthenticationWithCookies()
    .SetupAuthorization()
    .SetupServices()
    .SetupHttpClients()
    .AddPersistentKeyStorage(appBuilder.Configuration);

WebApplication app = appBuilder.Build();

_ = app
    .SetupExceptionHandler(app.Environment.IsDevelopment())
    .SetupStandardMiddlewares();

_ = app.MapRazorPages();

app.Run();
