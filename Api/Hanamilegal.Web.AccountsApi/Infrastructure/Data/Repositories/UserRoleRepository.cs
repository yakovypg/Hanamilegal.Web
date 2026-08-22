using System;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiConfiguration.Exceptions;
using Hanamilegal.Web.ApiConfiguration.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Hanamilegal.Web.AccountsApi.Infrastructure.Data.Repositories;

internal sealed class UserRoleRepository : IUserRoleRepository
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<UserRepository> _logger;

    internal UserRoleRepository(
        RoleManager<IdentityRole> roleManager,
        ILogger<UserRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(roleManager, nameof(roleManager));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<IdentityRole> AddAsync(string roleName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(roleName, nameof(roleName));

        _logger.LogInformation("Trying to add {RoleName} role", roleName);

        var role = new IdentityRole(roleName);

        IdentityResult createRoleResult = await _roleManager.CreateAsync(role);

        if (!createRoleResult.Succeeded)
        {
            string errorDescriptions = createRoleResult.Errors.GetJoinedDescriptions();
            _logger.LogWarning("Cannot add {RoleName} role: {@Errors}", roleName, errorDescriptions);

            throw new BadRequestException($"Cannot add {roleName} role: {errorDescriptions}");
        }

        _logger.LogInformation("{RoleName} role added", roleName);

        return role;
    }

    public async Task DeleteByNameAsync(string roleName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(roleName, nameof(roleName));

        _logger.LogInformation("Trying to delete {RoleName} role", roleName);

        IdentityRole? existingRole = await _roleManager.FindByNameAsync(roleName);

        if (existingRole is null)
        {
            _logger.LogWarning("{RoleName} role not found", roleName);
            throw new NotFoundException($"{roleName} role not found");
        }

        IdentityResult deleteRoleResult = await _roleManager.DeleteAsync(existingRole);

        if (!deleteRoleResult.Succeeded)
        {
            string errorDescriptions = deleteRoleResult.Errors.GetJoinedDescriptions();
            _logger.LogWarning("Cannot delete {RoleName} role: {@Errors}", roleName, errorDescriptions);

            throw new BadRequestException($"Cannot delete {roleName} role: {errorDescriptions}");
        }

        _logger.LogInformation("{RoleName} role deleted", roleName);
    }

    public async Task<IdentityRole?> FindByNameAsync(string roleName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(roleName, nameof(roleName));

        _logger.LogInformation("Trying to find {RoleName} role", roleName);

        IdentityRole? foundRole = await _roleManager.FindByNameAsync(roleName);

        _logger.LogInformation(
            "{RoleName} role found: {Found}",
            roleName,
            foundRole is not null);

        return foundRole;
    }

    public async Task<bool> ExistsByNameAsync(string roleName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(roleName, nameof(roleName));

        _logger.LogInformation("Trying to check if {RoleName} role exists", roleName);

        IdentityRole? existingRole = await FindByNameAsync(roleName);

        _logger.LogInformation(
            "{RoleName} role exists: {Exists}",
            roleName,
            existingRole is not null);

        return existingRole is not null;
    }
}
