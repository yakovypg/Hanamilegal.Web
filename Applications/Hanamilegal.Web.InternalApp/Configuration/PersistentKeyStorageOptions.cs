namespace Hanamilegal.Web.InternalApp.Configuration;

internal sealed record PersistentKeyStorageOptions(string Path, int LifetimeDays)
{
    public const string SectionName = "PersistentKeyStorage";

    public PersistentKeyStorageOptions()
        : this(string.Empty, default)
    { }
}
