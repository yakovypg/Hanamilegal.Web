using System;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.Requests;
using Hanamilegal.Web.Contracts.Applications;

namespace Hanamilegal.Web.ApplicationsApi.Services;

public interface IConsentsService
{
    Task<ConsentAuditDto> CreatePersonalDataConsentAuditAsync(
        RequestContext requestContext,
        CancellationToken cancellationToken = default);

    Task<ConsentAuditDto> AddExternalEntityIdToPersonalDataConsentAuditAsync(
        Guid consentAuditId,
        Guid externalEntityId,
        CancellationToken cancellationToken = default);
}
