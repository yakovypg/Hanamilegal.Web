using System;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Db;
using MongoDB.Driver;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Repositories;

internal sealed class ConsentsRepository : IConsentsRepository
{
    private readonly ConsentsDbContext _context;

    public ConsentsRepository(ConsentsDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        _context = context;
    }

    public async Task AddPersonalDataConsentAuditAsync(
        ConsentAudit consent,
        CancellationToken cancellationToken = default)
    {
        await _context.PersonalDataConsentAudits
            .InsertOneAsync(consent, null, cancellationToken);
    }

    public async Task<ConsentAudit> AddExternalEntityIdToPersonalDataConsentAuditAsync(
        Guid consentId,
        Guid externalEntityId,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConsentAudit>.Filter.Eq(t => t.Id, consentId);
        var update = Builders<ConsentAudit>.Update.Set(t => t.ExternalEntityId, externalEntityId);

        return await _context.PersonalDataConsentAudits
            .FindOneAndUpdateAsync(filter, update, null, cancellationToken);
    }
}
