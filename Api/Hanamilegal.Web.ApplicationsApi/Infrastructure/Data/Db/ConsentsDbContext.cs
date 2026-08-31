using System;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using MongoDB.Driver;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Db;

public class ConsentsDbContext
{
    private readonly IMongoDatabase _database;

    public ConsentsDbContext(IMongoClient mongoClient, string databaseName)
    {
        ArgumentNullException.ThrowIfNull(mongoClient, nameof(mongoClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName, nameof(databaseName));

        _database = mongoClient.GetDatabase(databaseName);
    }

    public IMongoCollection<ConsentAudit> PersonalDataConsentAudits =>
        _database.GetCollection<ConsentAudit>(nameof(PersonalDataConsentAudits));
}
