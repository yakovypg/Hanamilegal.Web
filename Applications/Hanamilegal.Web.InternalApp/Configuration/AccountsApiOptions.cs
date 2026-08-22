namespace Hanamilegal.Web.InternalApp.Configuration;

internal sealed record AccountsApiOptions(string BaseUrl)
{
    internal const string SectionName = "AccountsApi";
}
