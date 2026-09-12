namespace Hanamilegal.Web.Contracts.Consents;

public enum ConsentAuditSortFieldDto
{
    Id,
    ExternalEntityId,
    CreatedAtUtc,
    SessionId,
    IpAddress,
    UserAgent,
    RequestPath,
    DocumentName,
    DocumentHash,
    DocumentVersion
}
