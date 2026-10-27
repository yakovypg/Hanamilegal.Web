using System;

namespace Hanamilegal.Web.Contracts.Applications;

public sealed record ApplicationDto<TApplicationTypeDto>(
    Guid Id,
    DateTimeOffset CreatedAtUtc,
    TApplicationTypeDto Type,
    string SenderName,
    string Organization,
    string Email,
    string Text)

    where TApplicationTypeDto : struct, Enum;
