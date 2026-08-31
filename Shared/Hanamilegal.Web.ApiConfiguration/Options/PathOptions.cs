namespace Hanamilegal.Web.ApiConfiguration.Options;

public sealed record PathOptions(string Documents)
{
    public const string SectionName = "Path";

    public PathOptions()
        : this(string.Empty)
    { }
}
