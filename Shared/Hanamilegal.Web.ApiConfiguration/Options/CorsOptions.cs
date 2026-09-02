namespace Hanamilegal.Web.ApiConfiguration.Options;

public sealed class CorsOptions
{
    public const string SectionName = "Cors";
    public CorsPolicyOptions Frontend { get; init; } = new();
}
