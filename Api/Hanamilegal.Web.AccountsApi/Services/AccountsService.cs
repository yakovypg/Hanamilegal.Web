using System;
using System.Threading.Tasks;
using Hanamilegal.Web.Auth.Models;
using Hanamilegal.Web.Auth.Services;
using Hanamilegal.Web.Contracts.Accounts;
using Microsoft.Extensions.Logging;

namespace Hanamilegal.Web.AccountsApi.Services;

internal sealed class AccountsService : IAccountsService
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AccountsService> _logger;

    internal AccountsService(
        IAuthenticationService authenticateService,
        ITokenService tokenService,
        ILogger<AccountsService> logger)
    {
        ArgumentNullException.ThrowIfNull(authenticateService, nameof(authenticateService));
        ArgumentNullException.ThrowIfNull(tokenService, nameof(tokenService));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _authenticationService = authenticateService;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        _logger.LogInformation("Trying to log in user");

        AuthenticationResult authenticationResult = await _authenticationService.AuthenticateAsync(dto);
        AccessToken accessToken = _tokenService.CreateAccessToken(authenticationResult);

        _logger.LogInformation("User logged in");

        return new LoginResponseDto(accessToken.Token);
    }
}
