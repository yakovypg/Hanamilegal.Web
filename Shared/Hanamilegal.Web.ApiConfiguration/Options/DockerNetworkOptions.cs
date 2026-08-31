namespace Hanamilegal.Web.ApiConfiguration.Options;

public sealed record DockerNetworkOptions(string Subnet, string Gateway)
{
    public DockerNetworkOptions()
        : this(string.Empty, string.Empty)
    { }
}
