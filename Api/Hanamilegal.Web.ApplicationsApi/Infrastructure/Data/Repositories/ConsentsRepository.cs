using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.Filters;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Db;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Repositories;

internal sealed class ConsentsRepository : IConsentsRepository
{
    private readonly ConsentsDbContext _context;
    private readonly ILogger<ConsentsRepository> _logger;

    public ConsentsRepository(ConsentsDbContext context, ILogger<ConsentsRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _context = context;
        _logger = logger;
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Trying to get number of consents");

        var filter = Builders<ConsentAudit>.Filter.Empty;

        long consentsCount = await _context.PersonalDataConsentAudits
            .CountDocumentsAsync(filter, null, cancellationToken);

        _logger.LogInformation("Received number of consents: {ConsentsCount}", consentsCount);

        return consentsCount;
    }

    public async Task AddPersonalDataConsentAuditAsync(
        ConsentAudit consent,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Trying to add personal data consent audit");

        await _context.PersonalDataConsentAudits
            .InsertOneAsync(consent, null, cancellationToken);

        _logger.LogInformation("Personal data consent audit added");
    }

    public async Task<ConsentAudit> AddExternalEntityIdToPersonalDataConsentAuditAsync(
        Guid consentId,
        Guid externalEntityId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Trying to add external entity ID ({ExternalEntityId}) to personal data consent audit ({ConsentId})",
            externalEntityId,
            consentId);

        var filter = Builders<ConsentAudit>.Filter.Eq(t => t.Id, consentId);
        var update = Builders<ConsentAudit>.Update.Set(t => t.ExternalEntityId, externalEntityId);

        ConsentAudit consentAudit = await _context.PersonalDataConsentAudits
            .FindOneAndUpdateAsync(filter, update, null, cancellationToken);

        _logger.LogInformation(
            "External entity ID ({ExternalEntityId}) added to personal data consent audit ({ConsentId})",
            externalEntityId,
            consentId);

        return consentAudit;
    }

    public async Task<ConsentAudit?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Trying to find consent {ConsentId}", id);

        var filter = Builders<ConsentAudit>.Filter.Eq(t => t.Id, id);

        using IAsyncCursor<ConsentAudit> cursor = await _context.PersonalDataConsentAudits
            .FindAsync(filter, null, cancellationToken);

        ConsentAudit? foundConsent = await cursor.FirstOrDefaultAsync(cancellationToken);

        _logger.LogInformation("Consent found: {Found}", foundConsent is not null);

        return foundConsent;
    }

    public async Task<IEnumerable<ConsentAudit>> FindAsync(
        IEnumerable<IMongoFilter<ConsentAudit>>? filters = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Trying to find consents");

        FilterDefinition<ConsentAudit> filter = Builders<ConsentAudit>.Filter.Empty;
        FindOptions<ConsentAudit, ConsentAudit> options = new();

        foreach (IMongoFilter<ConsentAudit> currentFilter in filters ?? [])
        {
            filter = currentFilter.Apply(filter, options);
        }

        using IAsyncCursor<ConsentAudit> cursor = await _context.PersonalDataConsentAudits
            .FindAsync(filter, options, cancellationToken);

        List<ConsentAudit> foundConsents = await cursor.ToListAsync(cancellationToken);

        _logger.LogInformation("Consents found: {Found}", foundConsents.Count > 0);

        return foundConsents;
    }
}
