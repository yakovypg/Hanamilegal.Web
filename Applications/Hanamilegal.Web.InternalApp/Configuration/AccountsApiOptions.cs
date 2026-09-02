namespace Hanamilegal.Web.InternalApp.Configuration;

public sealed class AccountsApiOptions
{
    public const string SectionName = "AccountsApi";
    public string BaseUrl { get; init; } = string.Empty;
}
