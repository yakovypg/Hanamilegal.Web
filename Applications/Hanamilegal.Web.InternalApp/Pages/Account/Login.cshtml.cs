using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
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
using Microsoft.IdentityModel.Tokens;

namespace Hanamilegal.Web.InternalApp.Pages.Account;

public class LoginModel : PageModel
{
    private readonly AccountsApiClient _accountsApiClient;
    private readonly ITokenService _tokenService;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(
        AccountsApiClient accountsApiClient,
        ITokenService tokenService,
        ILogger<LoginModel> logger)
    {
        ArgumentNullException.ThrowIfNull(accountsApiClient, nameof(accountsApiClient));
        ArgumentNullException.ThrowIfNull(tokenService, nameof(tokenService));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _accountsApiClient = accountsApiClient;
        _tokenService = tokenService;
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

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        if (!ModelState.IsValid)
            return Page();

        LoginResponseDto loginResponse;

        try
        {
            var loginData = new LoginRequestDto()
            {
                Email = Login,
                Password = Password
            };

            loginResponse = await _accountsApiClient.LoginAsync(loginData);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Login failed: {ErrorMessage}", ex.Message);

            if (ex.StatusCode == HttpStatusCode.Unauthorized)
                ModelState.AddModelError(string.Empty, "Invalid email or password");
            else
                ModelState.AddModelError(string.Empty, "Authentication service is temporarily unavailable");

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed: {ErrorMessage}", ex.Message);
            ModelState.AddModelError(string.Empty, "Authentication service is temporarily unavailable");

            return Page();
        }

        JwtSecurityTokenHandler tokenHandler = new();
        TokenValidationParameters tokenValidationParameters = _tokenService.CreateTokenValidationParameters();

        TokenValidationResult tokenValidationResult = await tokenHandler.ValidateTokenAsync(
            loginResponse.AccessToken,
            tokenValidationParameters);

        if (!tokenValidationResult.IsValid)
        {
            _logger.LogWarning("Login failed: authentication token is invalid");
            ModelState.AddModelError(string.Empty, "The authentication token is invalid");
            return Page();
        }

        var claimsPrincipal = new ClaimsPrincipal(tokenValidationResult.ClaimsIdentity);

        if (tokenValidationResult.SecurityToken is not JwtSecurityToken validatedToken)
        {
            _logger.LogWarning("Login failed: authentication token is invalid");
            ModelState.AddModelError(string.Empty, "The authentication token is invalid");
            return Page();
        }

        var tokenExpiration = new DateTimeOffset(validatedToken.ValidTo, TimeSpan.Zero);

        var authenticationToken = new AuthenticationToken()
        {
            Name = AuthenticationTokenNames.AccessToken,
            Value = loginResponse.AccessToken
        };

        var authenticationProperties = new AuthenticationProperties()
        {
            AllowRefresh = true,
            IsPersistent = RememberMe,
            ExpiresUtc = tokenExpiration
        };

        authenticationProperties.StoreTokens([authenticationToken]);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal,
            authenticationProperties);

        return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? LocalRedirect(returnUrl)
            : RedirectToPage("/Index");
    }
}
