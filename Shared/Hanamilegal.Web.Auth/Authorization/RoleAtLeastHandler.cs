using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hanamilegal.Web.Auth.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hanamilegal.Web.Auth.Authorization;

public sealed class RoleAtLeastHandler : AuthorizationHandler<RoleAtLeastRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        RoleAtLeastRequirement requirement)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(requirement, nameof(requirement));

        IEnumerable<string> roleClaims = context.User
            .FindAll(ClaimTypes.Role)
            .Select(c => c.Value);

        var userRoles = roleClaims
            .Select(t => Enum.TryParse(t, true, out UserRole role) ? (UserRole?)role : null)
            .Where(t => t.HasValue)
            .Select(t => t!.Value);

        bool ok = userRoles.Any(t => t >= requirement.MinRole);

        if (ok)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
