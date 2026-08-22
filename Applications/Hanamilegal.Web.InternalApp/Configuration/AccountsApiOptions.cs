namespace Hanamilegal.Web.InternalApp.Configuration;

internal sealed record AccountsApiOptions(string BaseUrl)
{
    internal const string SectionName = "AccountsApi";

    public AccountsApiOptions()
        : this(string.Empty)
    { }
}
