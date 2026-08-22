using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hanamilegal.Web.AccountsApi.Infrastructure.Data.Repositories;
using Hanamilegal.Web.ApiConfiguration.Exceptions;
using Hanamilegal.Web.Auth.Models;
using Hanamilegal.Web.Contracts.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Hanamilegal.Web.AccountsApi.Services;

internal sealed class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<AuthenticationService> _logger;

    internal AuthenticationService(
        IUserRepository userRepository,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        ILogger<AuthenticationService> logger)
    {
        ArgumentNullException.ThrowIfNull(userRepository, nameof(userRepository));
        ArgumentNullException.ThrowIfNull(signInManager, nameof(signInManager));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _userRepository = userRepository;
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<AuthenticationResult> AuthenticateAsync(LoginRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        _logger.LogInformation("Trying to authenticate user");

        IdentityUser? user = await _userRepository.FindByEmailAsync(dto.Email);

        if (user is null)
        {
            _logger.LogWarning("Invalid email");
            throw new BadRequestException("Invalid email or password");
        }

        _logger.LogInformation("Trying to sign in user");

        SignInResult signInResult = await _signInManager
            .PasswordSignInAsync(user, dto.Password, false, false);

        if (!signInResult.Succeeded)
        {
            _logger.LogWarning("Invalid password");
            throw new BadRequestException("Invalid email or password");
        }

        _logger.LogInformation("User authenticated successfully");

        IReadOnlyList<string> roles = [.. await _userManager.GetRolesAsync(user)];

        return new AuthenticationResult(user.Id, user.Email, roles);
    }
}
