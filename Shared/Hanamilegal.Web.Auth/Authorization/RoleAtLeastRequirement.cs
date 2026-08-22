using Hanamilegal.Web.Auth.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hanamilegal.Web.Auth.Authorization;

public sealed class RoleAtLeastRequirement : IAuthorizationRequirement
{
    public RoleAtLeastRequirement(UserRole minRole)
    {
        MinRole = minRole;
    }

    public static RoleAtLeastRequirement RoleAtLeastUser => new(UserRole.User);
    public static RoleAtLeastRequirement RoleAtLeastApplicationViewer => new(UserRole.ApplicationViewer);
    public static RoleAtLeastRequirement RoleAtLeastAdmin => new(UserRole.Admin);

    public UserRole MinRole { get; }
}
