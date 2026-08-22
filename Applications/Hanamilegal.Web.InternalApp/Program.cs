using System;
using Hanamilegal.Web.ApiConfiguration.Extensions;
using Hanamilegal.Web.InternalApp.Api;
using Hanamilegal.Web.InternalApp.Api.Accounts;
using Hanamilegal.Web.InternalApp.Api.Applications;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(14);
        options.SlidingExpiration = true;
    });

builder.Services.SetupAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddJwtTokenService();

builder.Services.AddTransient<ApiAuthorizationHandler>();

builder.Services
    .AddOptions<AccountsApiOptions>()
    .BindConfiguration(AccountsApiOptions.SectionName)
    .Validate(o => !string.IsNullOrWhiteSpace(o.BaseUrl))
    .ValidateOnStart();

builder.Services
    .AddOptions<ApplicationsApiOptions>()
    .BindConfiguration(ApplicationsApiOptions.SectionName)
    .Validate(o => !string.IsNullOrWhiteSpace(o.BaseUrl))
    .ValidateOnStart();

builder.Services.AddHttpClient<AccountsApiClient>((serviceProvider, client) =>
{
    var options = serviceProvider
        .GetRequiredService<IOptions<AccountsApiOptions>>()
        .Value;

    client.BaseAddress = new Uri(options.BaseUrl);
});

builder.Services.AddHttpClient<ApplicationsApiClient>((serviceProvider, client) =>
{
    var options = serviceProvider
        .GetRequiredService<IOptions<ApplicationsApiOptions>>()
        .Value;

    client.BaseAddress = new Uri(options.BaseUrl);
}).AddHttpMessageHandler<ApiAuthorizationHandler>();

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    // app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
