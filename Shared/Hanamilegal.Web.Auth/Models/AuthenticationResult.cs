using System.Collections.Generic;

namespace Hanamilegal.Web.Auth.Models;

public record struct AuthenticationResult(
    string UserId,
    string? UserEmail,
    IReadOnlyList<string> UserRoles);
