using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.Auth.Services;
using Hanamilegal.Web.Contracts.Accounts;
using Hanamilegal.Web.InternalApp.Api.Accounts;
using Hanamilegal.Web.InternalApp.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Hanamilegal.Web.InternalApp.Pages.Account;

public class LoginModel : PageModel
{
    private readonly AccountsApiClient _accountsApiClient;
    private readonly IAccessTokenService _accessTokenService;
    private readonly IOptions<TokenLifetimeOptions> _tokenLifetimeOptions;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(
        AccountsApiClient accountsApiClient,
        IAccessTokenService accessTokenService,
        IOptions<TokenLifetimeOptions> tokenLifetimeOptions,
        ILogger<LoginModel> logger)
    {
        ArgumentNullException.ThrowIfNull(accountsApiClient, nameof(accountsApiClient));
        ArgumentNullException.ThrowIfNull(accessTokenService, nameof(accessTokenService));
        ArgumentNullException.ThrowIfNull(tokenLifetimeOptions, nameof(tokenLifetimeOptions));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _accountsApiClient = accountsApiClient;
        _accessTokenService = accessTokenService;
        _tokenLifetimeOptions = tokenLifetimeOptions;
        _logger = logger;
    }

    [BindProperty]
    [Required]
    [Display(Name = "Login")]
    public string Login { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }

    private LoginRequestDto LoginData => new() { Email = Login, Password = Password };

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(
        string? returnUrl = null,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        LoginResponseDto loginResponse;

        try
        {
            loginResponse = await _accountsApiClient.LoginAsync(LoginData, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            HandleHttpRequestException(ex);
            return Page();
        }
        catch (Exception ex)
        {
            HandleException(ex);
            return Page();
        }

        bool success = await SignInAsync(loginResponse);

        if (!success)
            return Page();

        return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? LocalRedirect(returnUrl)
            : RedirectToPage("/Index");
    }

    private void HandleHttpRequestException(HttpRequestException ex)
    {
        ArgumentNullException.ThrowIfNull(ex, nameof(ex));

        _logger.LogError(ex, "Login failed: {ErrorMessage}", ex.Message);

        if (ex.StatusCode == HttpStatusCode.Unauthorized)
            ModelState.AddModelError(string.Empty, "Invalid email or password");
        else
            ModelState.AddModelError(string.Empty, "Authentication service is temporarily unavailable");
    }

    private void HandleException(Exception ex)
    {
        ArgumentNullException.ThrowIfNull(ex, nameof(ex));

        _logger.LogError(ex, "Login failed: {ErrorMessage}", ex.Message);
        ModelState.AddModelError(string.Empty, "Authentication service is temporarily unavailable");
    }

    private async Task<bool> SignInAsync(LoginResponseDto loginResponse)
    {
        ArgumentNullException.ThrowIfNull(loginResponse, nameof(loginResponse));

        TokenValidationResult tokenValidationResult = await ValidateAccessTokenAsync(loginResponse.AccessToken);
        ClaimsPrincipal claimsPrincipal = new(tokenValidationResult.ClaimsIdentity);

        if (HasTokenValidationErrors(tokenValidationResult))
            return false;

        AuthenticationProperties authenticationProperties = CreateAuthenticationProperties(loginResponse);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal,
            authenticationProperties);

        return true;
    }

    private async Task<TokenValidationResult> ValidateAccessTokenAsync(string accessToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken, nameof(accessToken));

        JwtSecurityTokenHandler tokenHandler = new();
        TokenValidationParameters tokenValidationParameters = _accessTokenService.CreateTokenValidationParameters();

        return await tokenHandler.ValidateTokenAsync(accessToken, tokenValidationParameters);
    }

    private bool HasTokenValidationErrors(TokenValidationResult tokenValidationResult)
    {
        ArgumentNullException.ThrowIfNull(tokenValidationResult, nameof(tokenValidationResult));

        if (!tokenValidationResult.IsValid)
        {
            _logger.LogWarning("Login failed: authentication token is invalid");
            ModelState.AddModelError(string.Empty, "The authentication token is invalid");
            return true;
        }

        if (tokenValidationResult.SecurityToken is not JwtSecurityToken validatedToken)
        {
            _logger.LogWarning("Login failed: authentication token is invalid");
            ModelState.AddModelError(string.Empty, "The authentication token is invalid");
            return true;
        }

        return false;
    }

    private AuthenticationProperties CreateAuthenticationProperties(LoginResponseDto loginResponse)
    {
        ArgumentNullException.ThrowIfNull(loginResponse, nameof(loginResponse));

        var accessToken = new AuthenticationToken()
        {
            Name = AuthenticationTokenNames.AccessToken,
            Value = loginResponse.AccessToken
        };

        var refreshToken = new AuthenticationToken()
        {
            Name = AuthenticationTokenNames.RefreshToken,
            Value = loginResponse.RefreshToken
        };

        var authenticationProperties = new AuthenticationProperties()
        {
            AllowRefresh = true,
            IsPersistent = RememberMe,
            ExpiresUtc = RememberMe
                ? DateTimeOffset.UtcNow.Add(_tokenLifetimeOptions.Value.RememberMeLifetime)
                : DateTimeOffset.UtcNow.Add(_tokenLifetimeOptions.Value.DefaultLifetime)
        };

        authenticationProperties.StoreTokens([accessToken, refreshToken]);

        return authenticationProperties;
    }
}
