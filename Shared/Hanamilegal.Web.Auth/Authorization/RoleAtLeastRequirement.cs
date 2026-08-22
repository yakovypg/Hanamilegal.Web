using Microsoft.AspNetCore.Authorization;
using Hanamilegal.Web.Auth.Models;

namespace Hanamilegal.Web.Auth.Authorization;

public sealed class RoleAtLeastRequirement : IAuthorizationRequirement
{
    public static RoleAtLeastRequirement RoleAtLeastUser => new(UserRole.User);
    public static RoleAtLeastRequirement RoleAtLeastApplicationViewer => new(UserRole.ApplicationViewer);
    public static RoleAtLeastRequirement RoleAtLeastAdmin => new(UserRole.Admin);

    public RoleAtLeastRequirement(UserRole minRole)
    {
        MinRole = minRole;
    }

    public UserRole MinRole { get; }
}
