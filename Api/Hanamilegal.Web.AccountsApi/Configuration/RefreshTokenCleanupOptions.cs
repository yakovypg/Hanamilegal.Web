using System;

namespace Hanamilegal.Web.AccountsApi.Configuration;

public sealed class RefreshTokenCleanupOptions
{
    public const string SectionName = "RefreshTokenCleanup";

    public TimeSpan Interval { get; init; }
    public TimeSpan RevokedTokenRetention { get; init; }
}
