namespace Hanamilegal.Web.ApiConfiguration.Options;

public sealed record CorsOptions(CorsPolicyOptions Frontend)
{
    public const string SectionName = "Cors";

    public CorsOptions()
        : this(new CorsPolicyOptions())
    { }
}
