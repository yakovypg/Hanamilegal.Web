using System;
using System.Threading.Tasks;
using Hanamilegal.Web.AccountsApi.Domain.Entities;
using Hanamilegal.Web.AccountsApi.Infrastructure.Data.Repositories;
using Hanamilegal.Web.Auth.Models;
using Hanamilegal.Web.Auth.Options;
using Hanamilegal.Web.Auth.Services;
using Hanamilegal.Web.Contracts.Accounts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hanamilegal.Web.AccountsApi.Services;

public sealed class AccountsService : IAccountsService
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IAccessTokenService _accessTokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IRefreshTokenHasher _refreshTokenHasher;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IOptions<RefreshTokenOptions> _refreshTokenOptions;
    private readonly ILogger<AccountsService> _logger;

    public AccountsService(
        IAuthenticationService authenticateService,
        IAccessTokenService accessTokenService,
        IRefreshTokenService refreshTokenService,
        IRefreshTokenHasher refreshTokenHasher,
        IRefreshTokenRepository refreshTokenRepository,
        IOptions<RefreshTokenOptions> refreshTokenOptions,
        ILogger<AccountsService> logger)
    {
        ArgumentNullException.ThrowIfNull(authenticateService, nameof(authenticateService));
        ArgumentNullException.ThrowIfNull(accessTokenService, nameof(accessTokenService));
        ArgumentNullException.ThrowIfNull(refreshTokenService, nameof(refreshTokenService));
        ArgumentNullException.ThrowIfNull(refreshTokenHasher, nameof(refreshTokenHasher));
        ArgumentNullException.ThrowIfNull(refreshTokenRepository, nameof(refreshTokenRepository));
        ArgumentNullException.ThrowIfNull(refreshTokenOptions, nameof(refreshTokenOptions));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _authenticationService = authenticateService;
        _accessTokenService = accessTokenService;
        _refreshTokenService = refreshTokenService;
        _refreshTokenHasher = refreshTokenHasher;
        _refreshTokenRepository = refreshTokenRepository;
        _refreshTokenOptions = refreshTokenOptions;
        _logger = logger;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        _logger.LogInformation("Trying to log in user");

        AuthenticationResult authenticationResult = await _authenticationService.AuthenticateAsync(dto);
        AccessToken accessToken = _accessTokenService.CreateAccessToken(authenticationResult);

        string refreshToken = _refreshTokenService.CreateRefreshToken();
        string refreshTokenHash = _refreshTokenHasher.HashRefreshToken(refreshToken);

        RefreshToken refreshTokenEntity = CreateRefreshTokenEntity(
            refreshTokenHash,
            authenticationResult.UserId);

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);

        _logger.LogInformation("User logged in");

        return new LoginResponseDto(accessToken.Token, refreshToken);
    }

    public async Task<LoginResponseDto> RefreshAccessTokenAsync(RefreshAccessTokenRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        _logger.LogInformation("Trying to refresh access token");

        string oldRefreshTokenHash = _refreshTokenHasher.HashRefreshToken(dto.RefreshToken);
        RefreshToken? storedRefreshToken = await _refreshTokenRepository.FindByHashAsync(oldRefreshTokenHash);

        if (storedRefreshToken is null ||
            storedRefreshToken.IsRevoked ||
            storedRefreshToken.ExpiresAtUtc <= DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException();
        }

        AuthenticationResult authenticationResult = await _authenticationService
            .GetAuthenticationResultAsync(storedRefreshToken.UserId);

        // Rotate refresh token
        string newRefreshToken = _refreshTokenService.CreateRefreshToken();
        string newRefreshTokenHash = _refreshTokenHasher.HashRefreshToken(newRefreshToken);

        RefreshToken newRefreshTokenEntity = CreateRefreshTokenEntity(
            newRefreshTokenHash,
            storedRefreshToken.UserId);

        AccessToken accessToken = _accessTokenService.CreateAccessToken(authenticationResult);
        await _refreshTokenRepository.ReplaceAsync(storedRefreshToken, newRefreshTokenEntity);

        _logger.LogInformation("Access token refreshed");

        return new LoginResponseDto(accessToken.Token, newRefreshToken);
    }

    private RefreshToken CreateRefreshTokenEntity(string refreshTokenHash, string userId)
    {
        ArgumentException.ThrowIfNullOrEmpty(refreshTokenHash, nameof(refreshTokenHash));
        ArgumentException.ThrowIfNullOrWhiteSpace(userId, nameof(userId));

        DateTimeOffset createdAtUtc = DateTimeOffset.UtcNow;

        return new RefreshToken()
        {
            UserId = userId,
            TokenHash = refreshTokenHash,

            CreatedAtUtc = createdAtUtc,
            ExpiresAtUtc = createdAtUtc.Add(_refreshTokenOptions.Value.Lifetime),
            RevokedAtUtc = null
        };
    }
}
