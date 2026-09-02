using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.Contracts.Accounts;
using Hanamilegal.Web.InternalApp.Api.Accounts;
using Hanamilegal.Web.InternalApp.Configuration;
using Hanamilegal.Web.InternalApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Hanamilegal.Web.InternalApp.Api;

public sealed class ApiAuthorizationHandler : DelegatingHandler
{
    private readonly AccountsApiClient _accountsApiClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenRefreshCoordinator _tokenRefreshCoordinator;
    private readonly ILogger<ApiAuthorizationHandler> _logger;

    public ApiAuthorizationHandler(
        AccountsApiClient accountsApiClient,
        IHttpContextAccessor httpContextAccessor,
        ITokenRefreshCoordinator tokenRefreshCoordinator,
        ILogger<ApiAuthorizationHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(accountsApiClient, nameof(accountsApiClient));
        ArgumentNullException.ThrowIfNull(httpContextAccessor, nameof(httpContextAccessor));
        ArgumentNullException.ThrowIfNull(tokenRefreshCoordinator, nameof(tokenRefreshCoordinator));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _accountsApiClient = accountsApiClient;
        _httpContextAccessor = httpContextAccessor;
        _tokenRefreshCoordinator = tokenRefreshCoordinator;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        string? accessTokenBeforeRequest = await GetAccessTokenAsync();
        SetAccessToken(request, accessTokenBeforeRequest);

        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.Unauthorized)
            return response;

        _logger.LogInformation("Received an Unauthorized response. Attempting to refresh the access token");

        string? newAccessToken = await RefreshAccessTokenIfNeededAsync(
            accessTokenBeforeRequest,
            cancellationToken);

        if (string.IsNullOrWhiteSpace(newAccessToken))
        {
            _logger.LogInformation("Failed to refresh the access token");
            return response;
        }

        _logger.LogInformation("Access token refreshed");

        response.Dispose();
        SetAccessToken(request, newAccessToken);

        _logger.LogInformation("Trying to resend the request");

        // Resending a request is safe only if its contents can be submitted more than once
        return await base.SendAsync(request, cancellationToken);
    }

    private static void SetAccessToken(HttpRequestMessage request, string? accessToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        request.Headers.Authorization = null;

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

    private async Task<string?> RefreshAccessTokenIfNeededAsync(
        string? accessTokenBeforeRequest,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(accessTokenBeforeRequest))
            return null;

        string? refreshToken = await GetRefreshTokenAsync();

        if (string.IsNullOrWhiteSpace(refreshToken))
            return null;

        string lockKey = _tokenRefreshCoordinator.CreateLockKey(refreshToken);

        async Task<string?> RefreshAccessToken(CancellationToken cancellationToken)
        {
            /*
             * While the current request was waiting for the lock, another request
             * could have updated the token and written a new cookie
             */
            string? currentAccessToken = await GetAccessTokenAsync();

            if (!string.IsNullOrWhiteSpace(currentAccessToken) &&
                !string.Equals(currentAccessToken, accessTokenBeforeRequest, StringComparison.Ordinal))
            {
                return currentAccessToken;
            }

            /*
             * While the current request was waiting for the lock, another request
             * could have rotated the refresh token
             */
            string? currentRefreshToken = await GetRefreshTokenAsync();

            if (string.IsNullOrWhiteSpace(currentRefreshToken))
                return null;

            LoginResponseDto? loginResponse = await RefreshAccessTokenFromApiAsync(
                currentRefreshToken,
                cancellationToken);

            if (loginResponse is null)
                return null;

            bool signedIn = await SignInAsync(loginResponse);

            if (!signedIn)
                return null;

            return loginResponse.AccessToken;
        }

        return await _tokenRefreshCoordinator
            .RunAsync(lockKey, RefreshAccessToken, cancellationToken);
    }

    private async Task<LoginResponseDto?> RefreshAccessTokenFromApiAsync(
        string refreshToken,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken, nameof(refreshToken));

        var refreshAccessTokenRequest = new RefreshAccessTokenRequestDto()
        {
            RefreshToken = refreshToken
        };

        try
        {
            return await _accountsApiClient.RefreshAccessTokenAsync(
                refreshAccessTokenRequest,
                cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to refresh access token: {@Exception}", ex);
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

        if (!authenticateResult.Succeeded ||
            authenticateResult.Principal is null ||
            authenticateResult.Properties is null)
        {
            return false;
        }

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
