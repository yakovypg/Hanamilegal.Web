namespace Hanamilegal.Web.InternalApp.Configuration;

public sealed class ApplicationsApiOptions
{
    public const string SectionName = "ApplicationsApi";
    public string BaseUrl { get; init; } = string.Empty;
}
