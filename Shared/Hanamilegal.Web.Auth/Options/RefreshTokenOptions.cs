using System;

namespace Hanamilegal.Web.Auth.Options;

public sealed record RefreshTokenOptions(int BytesNumber, TimeSpan Lifetime)
{
    public const string SectionName = "RefreshToken";

    public RefreshTokenOptions()
        : this(32, TimeSpan.FromDays(30))
    { }
}
