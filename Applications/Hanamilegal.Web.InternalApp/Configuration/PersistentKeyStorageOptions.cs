using System;

namespace Hanamilegal.Web.InternalApp.Configuration;

public sealed class PersistentKeyStorageOptions
{
    public const string SectionName = "PersistentKeyStorage";

    public string Path { get; init; } = string.Empty;
    public TimeSpan Lifetime { get; init; }
}
