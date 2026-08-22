using System;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiConfiguration.Exceptions;
using Hanamilegal.Web.ApiConfiguration.Extensions;
using Hanamilegal.Web.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Hanamilegal.Web.AccountsApi.Infrastructure.Data.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(
        UserManager<IdentityUser> userManager,
        ILogger<UserRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(userManager, nameof(userManager));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _userManager = userManager;
        _logger = logger;
    }

    public async Task<IdentityUser> AddAsync(string email, string password, UserRole role)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));
        ArgumentException.ThrowIfNullOrEmpty(password, nameof(password));

        _logger.LogInformation("Trying to add user");

        var user = new IdentityUser()
        {
            Email = email,
            UserName = email
        };

        IdentityResult createUserResult = await _userManager.CreateAsync(user, password);

        if (!createUserResult.Succeeded)
        {
            string errorDescriptions = createUserResult.Errors.GetJoinedDescriptions();
            _logger.LogWarning("Cannot add user: {@Errors}", errorDescriptions);

            throw new BadRequestException($"Cannot add user: {errorDescriptions}");
        }

        _logger.LogInformation("User added");

        string roleName = role.ToString();

        _logger.LogInformation("Trying to add user to {RoleName} role", roleName);

        IdentityResult addToRoleResult = await _userManager.AddToRoleAsync(user, roleName);

        if (!addToRoleResult.Succeeded)
        {
            string errorDescriptions = addToRoleResult.Errors.GetJoinedDescriptions();
            _logger.LogWarning("Cannot add user to {RoleName} role: {@Errors}", roleName, errorDescriptions);

            throw new BadRequestException($"Cannot add user to {roleName} role: {errorDescriptions}");
        }

        _logger.LogInformation("User added to {RoleName} role", roleName);

        return user;
    }

    public async Task DeleteByEmailAsync(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));

        _logger.LogInformation("Trying to delete user");

        IdentityUser? existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser is null)
        {
            _logger.LogInformation("User not found");
            throw new NotFoundException("User not found");
        }

        IdentityResult deleteUserResult = await _userManager.DeleteAsync(existingUser);

        if (!deleteUserResult.Succeeded)
        {
            string errorDescriptions = deleteUserResult.Errors.GetJoinedDescriptions();
            _logger.LogWarning("Cannot delete user: {@Errors}", errorDescriptions);

            throw new BadRequestException("Cannot delete user: {errorDescriptions}");
        }

        _logger.LogInformation("User deleted");
    }

    public async Task<IdentityUser?> FindByEmailAsync(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));

        _logger.LogInformation("Trying to find user");

        IdentityUser? foundUser = await _userManager.FindByEmailAsync(email);

        _logger.LogInformation("User found: {Found}", foundUser is not null);

        return foundUser;
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));

        _logger.LogInformation("Trying to check if user exists");

        IdentityUser? existingUser = await FindByEmailAsync(email);

        _logger.LogInformation("User exists: {Exists}", existingUser is not null);

        return existingUser is not null;
    }
}
