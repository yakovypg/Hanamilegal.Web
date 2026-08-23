using System;
using Hanamilegal.Web.ApplicationsApi.Domain.Enums;

namespace Hanamilegal.Web.ApplicationsApi.Domain.Entities;

public sealed class Application
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

    public ApplicationType Type { get; set; }
    public string SenderName { get; set; }
    public string Organization { get; set; }
    public string Email { get; set; }
    public string Text { get; set; }
}
