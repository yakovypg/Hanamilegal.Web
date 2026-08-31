using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hanamilegal.Web.ApplicationsApi.Domain.Entities;

public sealed class ConsentAudit
{
    public ConsentAudit()
    {
        ExternalEntityId = null;
        CreatedAtUtc = DateTimeOffset.UtcNow;

        SessionId = string.Empty;
        IpAddress = string.Empty;
        UserAgent = string.Empty;
        RequestPath = string.Empty;

        DocumentName = string.Empty;
        DocumentHash = string.Empty;
        DocumentVersion = string.Empty;
    }

    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid? ExternalEntityId { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }

    public string SessionId { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public string RequestPath { get; set; }

    public string DocumentName { get; set; }
    public string DocumentHash { get; set; }
    public string DocumentVersion { get; set; }
}
