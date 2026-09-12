using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.Filters;
using Hanamilegal.Web.ApiCommon.Pagination;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Repositories;

public interface IConsentsRepository
{
    Task<long> CountAsync(
        IEnumerable<IMongoFilter<ConsentAudit>>? filters = null,
        CancellationToken cancellationToken = default);

    Task AddPersonalDataConsentAuditAsync(
        ConsentAudit consent,
        CancellationToken cancellationToken = default);

    Task<ConsentAudit> AddExternalEntityIdToPersonalDataConsentAuditAsync(
        Guid consentId,
        Guid externalEntityId,
        CancellationToken cancellationToken = default);

    Task<ConsentAudit?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<PaginationResult<ConsentAudit>> FindAsync(
        MongoPaginationFilter<ConsentAudit> paginationFilter,
        IEnumerable<IMongoFilter<ConsentAudit>>? filters = null,
        CancellationToken cancellationToken = default);
}
