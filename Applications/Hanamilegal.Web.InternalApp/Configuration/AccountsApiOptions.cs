namespace Hanamilegal.Web.InternalApp.Configuration;

public sealed record AccountsApiOptions(string BaseUrl)
{
    public const string SectionName = "AccountsApi";

    public AccountsApiOptions()
        : this(string.Empty)
    { }
}
