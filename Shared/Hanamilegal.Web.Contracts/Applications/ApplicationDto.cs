using System;

namespace Hanamilegal.Web.Contracts.Applications;

public sealed record ApplicationDto(
    Guid Id,
    DateTimeOffset CreatedAtUtc,
    ApplicationTypeDto Type,
    string SenderName,
    string Organization,
    string Email,
    string Text);
