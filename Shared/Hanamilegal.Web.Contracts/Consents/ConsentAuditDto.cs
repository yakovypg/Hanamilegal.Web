using System;
using Hanamilegal.Web.Contracts.Applications;

namespace Hanamilegal.Web.Contracts.Consents;

public sealed record ConsentAuditDto(
    Guid Id,
    Guid? ExternalEntityId,
    ApplicationProviderDto ApplicationProvider,
    DateTimeOffset CreatedAtUtc,
    string SessionId,
    string IpAddress,
    string UserAgent,
    string RequestPath,
    string DocumentName,
    string DocumentHash,
    string DocumentVersion);
