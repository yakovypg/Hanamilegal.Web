using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.Contracts.Accounts;
using Hanamilegal.Web.InternalApp.Api.Accounts;
using Hanamilegal.Web.InternalApp.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace Hanamilegal.Web.InternalApp.Api;

public sealed class ApiAuthorizationHandler : DelegatingHandler
{
    private readonly AccountsApiClient _accountsApiClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiAuthorizationHandler(
        AccountsApiClient accountsApiClient,
        IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(accountsApiClient, nameof(accountsApiClient));
        ArgumentNullException.ThrowIfNull(httpContextAccessor, nameof(httpContextAccessor));

        _accountsApiClient = accountsApiClient;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        await AddAccessTokenAsync(request);
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.Unauthorized)
            return response;

        response.Dispose();

        bool accessTokenRefreshed = await RefreshAccessTokenAsync(cancellationToken);

        if (!accessTokenRefreshed)
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);

        await AddAccessTokenAsync(request);
        return await base.SendAsync(request, cancellationToken);
    }

    private static void AddAccessToken(HttpRequestMessage request, string? accessToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        if (string.IsNullOrWhiteSpace(accessToken))
            return;

        request.Headers.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            accessToken);
    }

    private static bool UpdateTokens(AuthenticateResult authenticateResult, LoginResponseDto loginResponse)
    {
        ArgumentNullException.ThrowIfNull(authenticateResult, nameof(authenticateResult));
        ArgumentNullException.ThrowIfNull(loginResponse, nameof(loginResponse));

        if (!authenticateResult.Succeeded || authenticateResult.Properties is null)
            return false;

        authenticateResult.Properties.UpdateTokenValue(
            AuthenticationTokenNames.AccessToken,
            loginResponse.AccessToken);

        authenticateResult.Properties.UpdateTokenValue(
            AuthenticationTokenNames.RefreshToken,
            loginResponse.RefreshToken);

        return true;
    }

    private async Task AddAccessTokenAsync(HttpRequestMessage request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        string? accessToken = await GetAccessTokenAsync();
        AddAccessToken(request, accessToken);
    }

    private async Task<string?> GetAccessTokenAsync()
    {
        HttpContext? httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is null)
            return null;

        return await httpContext.GetTokenAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            AuthenticationTokenNames.AccessToken);
    }

    private async Task<string?> GetRefreshTokenAsync()
    {
        HttpContext? httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is null)
            return null;

        return await httpContext.GetTokenAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            AuthenticationTokenNames.RefreshToken);
    }

    private async Task<bool> RefreshAccessTokenAsync(CancellationToken cancellationToken)
    {
        string? refreshToken = await GetRefreshTokenAsync();
        LoginResponseDto? loginResponse = await RefreshAccessTokenAsync(refreshToken, cancellationToken);

        return loginResponse is not null && await SignInAsync(loginResponse);
    }

    private async Task<LoginResponseDto?> RefreshAccessTokenAsync(
        string? refreshToken,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return null;

        var refreshAccessTokenRequest = new RefreshAccessTokenRequestDto(refreshToken);

        try
        {
            return await _accountsApiClient.RefreshAccessTokenAsync(
                refreshAccessTokenRequest,
                cancellationToken);
        }
        catch
        {
            return null;
        }
    }

    private async Task<bool> SignInAsync(LoginResponseDto loginResponse)
    {
        ArgumentNullException.ThrowIfNull(loginResponse, nameof(loginResponse));

        HttpContext? httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is null)
            return false;

        AuthenticateResult authenticateResult = await httpContext
            .AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!authenticateResult.Succeeded || authenticateResult.Properties is null)
            return false;

        bool tokensUpdated = UpdateTokens(authenticateResult, loginResponse);

        if (!tokensUpdated)
            return false;

        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            authenticateResult.Principal,
            authenticateResult.Properties);

        return true;
    }
}
