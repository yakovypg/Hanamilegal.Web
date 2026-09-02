using System.Collections.Generic;

namespace Hanamilegal.Web.Auth.Models;

public record AuthenticationResult(
    string UserId,
    string? UserEmail,
    IReadOnlyList<string> UserRoles);
