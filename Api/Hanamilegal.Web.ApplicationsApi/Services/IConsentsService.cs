using System;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.Pagination;
using Hanamilegal.Web.ApiCommon.Requests;
using Hanamilegal.Web.ApplicationsApi.Domain.Enums;
using Hanamilegal.Web.Contracts.Consents;

namespace Hanamilegal.Web.ApplicationsApi.Services;

public interface IConsentsService
{
    Task<ConsentAuditDto> CreatePersonalDataConsentAuditAsync(
        ApplicationProvider applicationProvider,
        RequestContext requestContext,
        CancellationToken cancellationToken = default);

    Task<ConsentAuditDto> AddExternalEntityIdToPersonalDataConsentAuditAsync(
        Guid consentAuditId,
        Guid externalEntityId,
        CancellationToken cancellationToken = default);

    Task<ConsentAuditDto?> GetByIdAsync(Guid id);
    Task<PaginationResult<ConsentAuditDto>> SearchAsync(ConsentAuditSearchRequestDto dto);
}
