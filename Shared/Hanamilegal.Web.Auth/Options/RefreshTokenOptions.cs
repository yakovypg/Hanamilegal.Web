using System;

namespace Hanamilegal.Web.Auth.Options;

public sealed class RefreshTokenOptions
{
    public const string SectionName = "RefreshToken";

    public int BytesNumber { get; init; }
    public TimeSpan Lifetime { get; init; }
}
