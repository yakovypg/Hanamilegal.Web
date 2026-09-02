namespace Hanamilegal.Web.ApiConfiguration.Options;

public sealed class DockerNetworkOptions
{
    public string Subnet { get; init; } = string.Empty;
    public string Gateway { get; init; } = string.Empty;
}
