namespace Hanamilegal.Web.ApiConfiguration.Options;

public sealed record FileNameOptions(
    string CookiePolicyName,
    string CookiePolicyManifestName,
    string PrivacyPolicyName,
    string PrivacyPolicyManifestName)
{
    public const string SectionName = "FileName";

    public FileNameOptions()
        : this(string.Empty, string.Empty, string.Empty, string.Empty)
    { }
}
