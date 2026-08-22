using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hanamilegal.Web.AccountsApi.Infrastructure.Data.Repositories;
using Hanamilegal.Web.Auth.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hanamilegal.Web.AccountsApi.Infrastructure.Data.Db;

internal sealed class AccountsDbInitializer : IAccountsDbInitializer
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AccountsDbInitializer> _logger;

    internal AccountsDbInitializer(
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository,
        IConfiguration configuration,
        ILogger<AccountsDbInitializer> logger)
    {
        ArgumentNullException.ThrowIfNull(userRepository, nameof(userRepository));
        ArgumentNullException.ThrowIfNull(userRoleRepository, nameof(userRoleRepository));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _configuration = configuration;
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

        IEnumerable<(string Email, string Password, UserRole Role)> initialUsers =
            AccountsDbInitialData.GetInitialUsers(_configuration);

        foreach (var (email, password, role) in initialUsers)
        {
            await AddUserAsync(email, password, role);
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
