namespace Hanamilegal.Web.ApiConfiguration.Options;

public sealed class FileNameOptions
{
    public const string SectionName = "FileName";

    public string CookiePolicyName { get; init; } = string.Empty;
    public string CookiePolicyManifestName { get; init; } = string.Empty;
    public string PrivacyPolicyName { get; init; } = string.Empty;
    public string PrivacyPolicyManifestName { get; init; } = string.Empty;
}
