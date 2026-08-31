using System.Text.Json.Serialization;

namespace Hanamilegal.Web.ApiConfiguration.FileSystem;

public sealed class DocumentManifest
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("version")]
    public string Version { get; init; } = string.Empty;

    [JsonPropertyName("fileName")]
    public string FileName { get; init; } = string.Empty;

    [JsonPropertyName("sha256")]
    public string Sha256 { get; init; } = string.Empty;
}
