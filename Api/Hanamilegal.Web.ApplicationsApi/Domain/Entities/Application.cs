using System;
using Hanamilegal.Web.ApplicationsApi.Domain.Enums;

namespace Hanamilegal.Web.ApplicationsApi.Domain.Entities;

internal sealed class Application
{
    internal Application()
    {
        CreatedAtUtc = DateTimeOffset.UtcNow;
        SenderName = string.Empty;
        Organization = string.Empty;
        Email = string.Empty;
        Text = string.Empty;
    }

    internal Guid Id { get; set; }
    internal DateTimeOffset CreatedAtUtc { get; set; }

    internal ApplicationType Type { get; set; }
    internal string SenderName { get; set; }
    internal string Organization { get; set; }
    internal string Email { get; set; }
    internal string Text { get; set; }
}
