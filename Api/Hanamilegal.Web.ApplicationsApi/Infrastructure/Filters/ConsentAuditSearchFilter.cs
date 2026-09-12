using System;
using System.Collections.Generic;
using Hanamilegal.Web.ApiCommon.Filters;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using MongoDB.Driver;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;

public sealed class ConsentAuditSearchFilter : IMongoFilter<ConsentAudit>
{
    public Guid? Id { get; set; }
    public Guid? ExternalEntityId { get; set; }
    public DateTimeOffset? FromDateUtc { get; set; }
    public DateTimeOffset? ToDateUtc { get; set; }
    public string? SessionId { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? RequestPath { get; set; }
    public string? DocumentName { get; set; }
    public string? DocumentHash { get; set; }
    public string? DocumentVersion { get; set; }

    public FilterDefinition<ConsentAudit> Apply(
        FilterDefinition<ConsentAudit> filter,
        FindOptions<ConsentAudit, ConsentAudit> options)
    {
        ArgumentNullException.ThrowIfNull(filter, nameof(filter));
        ArgumentNullException.ThrowIfNull(options, nameof(options));

        var builder = Builders<ConsentAudit>.Filter;
        var filters = new List<FilterDefinition<ConsentAudit>>();

        if (Id is not null)
        {
            filters.Add(builder.Eq(
                audit => audit.Id,
                Id));
        }

        if (ExternalEntityId is not null)
        {
            filters.Add(builder.Eq(
                audit => audit.ExternalEntityId,
                ExternalEntityId));
        }

        if (FromDateUtc is not null)
        {
            filters.Add(builder.Gte(
                audit => audit.CreatedAtUtc,
                FromDateUtc));
        }

        if (ToDateUtc is not null)
        {
            filters.Add(builder.Lte(
                audit => audit.CreatedAtUtc,
                ToDateUtc));
        }

        if (SessionId is not null)
        {
            filters.Add(builder.Eq(
                audit => audit.SessionId,
                SessionId));
        }

        if (IpAddress is not null)
        {
            filters.Add(builder.Eq(
                audit => audit.IpAddress,
                IpAddress));
        }

        if (UserAgent is not null)
        {
            filters.Add(builder.Eq(
                audit => audit.UserAgent,
                UserAgent));
        }

        if (RequestPath is not null)
        {
            filters.Add(builder.Eq(
                audit => audit.RequestPath,
                RequestPath));
        }

        if (DocumentName is not null)
        {
            filters.Add(builder.Eq(
                audit => audit.DocumentName,
                DocumentName));
        }

        if (DocumentHash is not null)
        {
            filters.Add(builder.Eq(
                audit => audit.DocumentHash,
                DocumentHash));
        }

        if (DocumentVersion is not null)
        {
            filters.Add(builder.Eq(
                audit => audit.DocumentVersion,
                DocumentVersion));
        }

        return filters.Count > 0
            ? builder.And(filter, builder.And(filters))
            : filter;
    }
}
