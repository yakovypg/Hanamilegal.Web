using System;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiConfiguration.Exceptions;
using Hanamilegal.Web.ApiConfiguration.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Hanamilegal.Web.AccountsApi.Infrastructure.Data.Repositories;

internal sealed partial class UserRoleRepository : IUserRoleRepository
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

        LogTryAddRole(roleName);

        var role = new IdentityRole(roleName);
        IdentityResult createRoleResult = await _roleManager.CreateAsync(role);

        if (!createRoleResult.Succeeded)
        {
            string errorDescriptions = createRoleResult.Errors.GetJoinedDescriptions();
            _logger.LogWarning("Cannot add {RoleName} role: {@Errors}", roleName, errorDescriptions);

            throw new BadRequestException($"Cannot add {roleName} role: {errorDescriptions}");
        }

        LogRoleAdded(roleName);

        return role;
    }

    public async Task DeleteByNameAsync(string roleName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(roleName, nameof(roleName));

        LogTryDeleteRole(roleName);

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

        LogRoleDeleted(roleName);
    }

    public async Task<IdentityRole?> FindByNameAsync(string roleName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(roleName, nameof(roleName));

        LogTryFindRole(roleName);
        IdentityRole? foundRole = await _roleManager.FindByNameAsync(roleName);
        LogFindRoleResult(roleName, foundRole is not null);

        return foundRole;
    }

    public async Task<bool> ExistsByNameAsync(string roleName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(roleName, nameof(roleName));

        LogTryCheckRoleExistsRole(roleName);
        IdentityRole? existingRole = await FindByNameAsync(roleName);
        LogRoleExistsResult(roleName, existingRole is not null);

        return existingRole is not null;
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Trying to add {RoleName} role")]
    private partial void LogTryAddRole(string roleName);

    [LoggerMessage(Level = LogLevel.Information, Message = "{RoleName} role added")]
    private partial void LogRoleAdded(string roleName);

    [LoggerMessage(Level = LogLevel.Information, Message = "Trying to delete {RoleName} role")]
    private partial void LogTryDeleteRole(string roleName);

    [LoggerMessage(Level = LogLevel.Information, Message = "{RoleName} role deleted")]
    private partial void LogRoleDeleted(string roleName);

    [LoggerMessage(Level = LogLevel.Information, Message = "Trying to find {RoleName} role")]
    private partial void LogTryFindRole(string roleName);

    [LoggerMessage(Level = LogLevel.Information, Message = "{RoleName} role found: {Found}")]
    private partial void LogFindRoleResult(string roleName, bool found);

    [LoggerMessage(Level = LogLevel.Information, Message = "Trying to check if {RoleName} role exists")]
    private partial void LogTryCheckRoleExistsRole(string roleName);

    [LoggerMessage(Level = LogLevel.Information, Message = "{RoleName} role exists: {Exists}")]
    private partial void LogRoleExistsResult(string roleName, bool exists);
}
