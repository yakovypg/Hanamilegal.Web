using System;
using Microsoft.AspNetCore.Identity;

namespace Hanamilegal.Web.ApiConfiguration.Extensions;

public static class IdentityOptionsExtensions
{
    public static void DisablePasswordRequirements(this IdentityOptions options)
    {
        ArgumentNullException.ThrowIfNull(options, nameof(options));

        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 0;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    }
}
