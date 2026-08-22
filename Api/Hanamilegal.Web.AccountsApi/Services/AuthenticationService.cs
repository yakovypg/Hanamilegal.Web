using System;
using System.Threading.Tasks;
using Hanamilegal.Web.AccountsApi.Contracts;
using Hanamilegal.Web.AccountsApi.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Hanamilegal.Web.AccountsApi.Services;

internal class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILogger<AuthenticationService> _logger;

    internal AuthenticationService(
        IUserRepository userRepository,
        SignInManager<IdentityUser> signInManager,
        ILogger<AuthenticationService> logger)
    {
        ArgumentNullException.ThrowIfNull(userRepository, nameof(userRepository));
        ArgumentNullException.ThrowIfNull(signInManager, nameof(signInManager));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _userRepository = userRepository;
        _signInManager = signInManager;
        _logger = logger;
    }
    
    public async Task<(bool Ok, string UserId)> AuthenticateAsync(LoginRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        _logger.LogInformation("Trying to authenticate user");

        IdentityUser? user = await _userRepository.FindByEmailAsync(dto.Email);

        if (user is null)
            return (false, string.Empty);

        _logger.LogInformation("Trying to sign in user");

        SignInResult signInResult = await _signInManager
            .PasswordSignInAsync(user, dto.Password, false, false);

        if (!signInResult.Succeeded)
        {
            _logger.LogWarning("User is not authenticated");
            return (false, string.Empty);
        }

        _logger.LogInformation("User authenticated successfully");

        return (true, user.Id);
    }
}
