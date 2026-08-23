using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.InternalApp.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace Hanamilegal.Web.InternalApp.Api;

public sealed class ApiAuthorizationHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor, nameof(httpContextAccessor));
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        HttpContext? httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is not null)
        {
            string? accessToken = await httpContext.GetTokenAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                AuthenticationTokenNames.AccessToken);

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(
                    JwtBearerDefaults.AuthenticationScheme,
                    accessToken);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
