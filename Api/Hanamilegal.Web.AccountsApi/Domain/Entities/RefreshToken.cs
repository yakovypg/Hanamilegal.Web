using System;

namespace Hanamilegal.Web.AccountsApi.Domain.Entities;

public sealed class RefreshToken
{
    public RefreshToken()
    {
        UserId = string.Empty;
        TokenHash = string.Empty;
        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string TokenHash { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset ExpiresAtUtc { get; set; }

    public DateTimeOffset? RevokedAtUtc { get; set; }
    public string? ReplacedByTokenHash { get; set; }

    public bool IsRevoked => RevokedAtUtc is not null;
}
