namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;

public enum ConsentAuditSortField
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
