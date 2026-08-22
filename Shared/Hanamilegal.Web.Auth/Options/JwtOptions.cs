namespace Hanamilegal.Web.Auth.Options;

public sealed record JwtOptions(
    string Key,
    string Issuer,
    string Audience,
    int ExpireMinutes,
    int ClockSkewSeconds)
{
    public const string SectionName = "Jwt";
}
