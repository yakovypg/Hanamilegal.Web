using System;

namespace Hanamilegal.Web.InternalApp.Configuration;

public sealed record PersistentKeyStorageOptions(string Path, TimeSpan Lifetime)
{
    public const string SectionName = "PersistentKeyStorage";

    public PersistentKeyStorageOptions()
        : this(string.Empty, TimeSpan.FromDays(14))
    { }
}
