using System;

namespace Hanamilegal.Web.Contracts.Consents;

public sealed record ConsentAuditDto(
    Guid Id,
    Guid? ExternalEntityId,
    DateTimeOffset CreatedAtUtc,
    string SessionId,
    string IpAddress,
    string UserAgent,
    string RequestPath,
    string DocumentName,
    string DocumentHash,
    string DocumentVersion);
