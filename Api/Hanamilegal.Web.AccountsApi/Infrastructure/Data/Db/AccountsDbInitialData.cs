using System;
using System.Collections.Generic;
using System.Linq;
using Hanamilegal.Web.Auth.Models;
using Microsoft.Extensions.Configuration;
using Hanamilegal.Web.ApiConfiguration.Extensions;

namespace Hanamilegal.Web.AccountsApi.Infrastructure.Data.Db;

internal static class AccountsDbInitialData
{
    internal static IEnumerable<(string Email, string Password, UserRole Role)> GetInitialUsers(
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        return Enum.GetNames<UserRole>()
            .Select(t => GetInitialUser(t, configuration));
    }

    internal static (string Email, string Password, UserRole Role) GetInitialUser(
        string userConfigName,
        IConfiguration configuration)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userConfigName, nameof(userConfigName));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        string email = configuration.GetString($"InitialUsers:{userConfigName}:Email");
        string password = configuration.GetString($"InitialUsers:{userConfigName}:Password");
        UserRole role = configuration.GetEnum<UserRole>($"InitialUsers:{userConfigName}:Role");

        return (email, password, role);
    }
}
