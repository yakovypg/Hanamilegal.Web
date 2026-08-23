namespace Hanamilegal.Web.InternalApp.Configuration;

public sealed record ApplicationsApiOptions(string BaseUrl)
{
    public const string SectionName = "ApplicationsApi";

    public ApplicationsApiOptions()
        : this(string.Empty)
    { }
}
