using System;

namespace Hanamilegal.Web.InternalApp.Configuration;

public sealed class TokenLifetimeOptions
{
    public const string SectionName = "TokenLifetime";

    public TimeSpan DefaultLifetime { get; init; } = TimeSpan.FromHours(8);
    public TimeSpan RememberMeLifetime { get; init; } = TimeSpan.FromDays(30);
}
