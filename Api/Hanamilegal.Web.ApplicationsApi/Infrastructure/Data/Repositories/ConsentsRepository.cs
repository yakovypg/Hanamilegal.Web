using System;
using System.Threading;
using System.Threading.Tasks;
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
}
