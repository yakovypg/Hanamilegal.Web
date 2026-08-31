namespace Hanamilegal.Web.ApiConfiguration.Options;

public sealed record DockerOptions(DockerNetworkOptions Network)
{
    public const string SectionName = "Docker";

    public DockerOptions()
        : this(new DockerNetworkOptions())
    { }
}
