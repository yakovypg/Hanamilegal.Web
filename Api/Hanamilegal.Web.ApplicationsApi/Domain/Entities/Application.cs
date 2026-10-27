using System;

namespace Hanamilegal.Web.ApplicationsApi.Domain.Entities;

public sealed class Application<TApplicationType>
    where TApplicationType : struct, Enum
{
    public Application()
    {
        CreatedAtUtc = DateTimeOffset.UtcNow;
        SenderName = string.Empty;
        Organization = string.Empty;
        Email = string.Empty;
        Text = string.Empty;
    }

    public Guid Id { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }

    public TApplicationType Type { get; set; }
    public string SenderName { get; set; }
    public string Organization { get; set; }
    public string Email { get; set; }
    public string Text { get; set; }
}
