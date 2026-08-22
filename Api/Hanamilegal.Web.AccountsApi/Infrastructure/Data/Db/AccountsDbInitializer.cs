using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hanamilegal.Web.AccountsApi.Configuration;
using Hanamilegal.Web.AccountsApi.Infrastructure.Data.Repositories;
using Hanamilegal.Web.Auth.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hanamilegal.Web.AccountsApi.Infrastructure.Data.Db;

internal sealed class AccountsDbInitializer : IAccountsDbInitializer
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IOptions<InitialUsersOptions> _initialUsersOptions;
    private readonly ILogger<AccountsDbInitializer> _logger;

    public AccountsDbInitializer(
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository,
        IOptions<InitialUsersOptions> initialUsersOptions,
        ILogger<AccountsDbInitializer> logger)
    {
        ArgumentNullException.ThrowIfNull(userRepository, nameof(userRepository));
        ArgumentNullException.ThrowIfNull(userRoleRepository, nameof(userRoleRepository));
        ArgumentNullException.ThrowIfNull(initialUsersOptions, nameof(initialUsersOptions));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _initialUsersOptions = initialUsersOptions;
        _logger = logger;
    }

    public async Task InitializeDatabaseAsync()
    {
        _logger.LogInformation("Start database initialization");

        await AddInitialDataAsync();

        _logger.LogInformation("Database initialized");
    }

    private async Task AddInitialDataAsync()
    {
        _logger.LogInformation("Start adding initial data");

        await AddInitialRolesAsync();
        await AddInitialUsersAsync();

        _logger.LogInformation("Initial data added");
    }

    private async Task AddInitialRolesAsync()
    {
        _logger.LogInformation("Start adding initial roles");

        string[] roleNames = Enum.GetNames<UserRole>();

        foreach (string roleName in roleNames)
        {
            await AddRoleAsync(roleName);
        }

        _logger.LogInformation("Initial roles added");
    }

    private async Task AddRoleAsync(string roleName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(roleName, nameof(roleName));

        bool roleExists = await _userRoleRepository.ExistsByNameAsync(roleName);

        if (roleExists)
            return;

        _ = await _userRoleRepository.AddAsync(roleName);
    }

    private async Task AddInitialUsersAsync()
    {
        _logger.LogInformation("Start adding initial users");

        List<InitialUser> initialUsers = _initialUsersOptions.Value.Users;

        foreach (InitialUser user in initialUsers)
        {
            await AddUserAsync(user.Email, user.Password, user.Role);
        }

        _logger.LogInformation("Initial users added");
    }

    private async Task AddUserAsync(string email, string password, UserRole role)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));
        ArgumentException.ThrowIfNullOrEmpty(password, nameof(password));

        bool userExists = await _userRepository.ExistsByEmailAsync(email);

        if (userExists)
            return;

        _ = await _userRepository.AddAsync(email, password, role);
    }
}
