namespace Hanamilegal.Web.ApiConfiguration.Options;

public sealed class DockerOptions
{
    public const string SectionName = "Docker";
    public DockerNetworkOptions Network { get; init; } = new();
}
