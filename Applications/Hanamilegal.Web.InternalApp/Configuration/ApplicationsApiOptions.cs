namespace Hanamilegal.Web.InternalApp.Configuration;

internal sealed record ApplicationsApiOptions(string BaseUrl)
{
    internal const string SectionName = "ApplicationsApi";
}
