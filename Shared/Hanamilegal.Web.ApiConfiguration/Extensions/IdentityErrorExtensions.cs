using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Hanamilegal.Web.ApiConfiguration.Extensions;

public static class IdentityErrorExtensions
{
    public static string GetJoinedDescriptions(this IEnumerable<IdentityError> errors)
    {
        ArgumentNullException.ThrowIfNull(errors, nameof(errors));

        IEnumerable<string> descriptions = errors.Select(t => t.Description);
        return string.Join(", ", descriptions);
    }
}
