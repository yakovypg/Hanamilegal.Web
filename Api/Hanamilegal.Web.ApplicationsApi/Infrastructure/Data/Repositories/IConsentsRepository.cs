using System;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Repositories;

public interface IConsentsRepository
{
    Task AddPersonalDataConsentAuditAsync(
        ConsentAudit consent,
        CancellationToken cancellationToken = default);

    Task<ConsentAudit> AddExternalEntityIdToPersonalDataConsentAuditAsync(
        Guid consentId,
        Guid externalEntityId,
        CancellationToken cancellationToken = default);
}
