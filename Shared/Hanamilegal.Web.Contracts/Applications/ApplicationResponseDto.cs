using System;

namespace Hanamilegal.Web.Contracts.Applications;

public sealed class ApplicationResponseDto
{
    public ApplicationResponseDto()
    {
        SenderName = string.Empty;
        Organization = string.Empty;
        Email = string.Empty;
        Text = string.Empty;
    }

    public Guid Id { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public ApplicationTypeDto Type { get; set; }
    public string SenderName { get; set; }
    public string Organization { get; set; }
    public string Email { get; set; }
    public string Text { get; set; }
}
