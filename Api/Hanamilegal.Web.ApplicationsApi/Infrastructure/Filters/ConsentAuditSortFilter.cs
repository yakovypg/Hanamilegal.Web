using System;
using Hanamilegal.Web.ApiCommon.Filters;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using MongoDB.Driver;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;

public sealed class ConsentAuditSortFilter : IMongoFilter<ConsentAudit>
{
    public ConsentAuditSortField SortBy { get; set; }
    public SortDirection Direction { get; set; }

    public FilterDefinition<ConsentAudit> Apply(
        FilterDefinition<ConsentAudit> filter,
        FindOptions<ConsentAudit, ConsentAudit> options)
    {
        ArgumentNullException.ThrowIfNull(filter, nameof(filter));
        ArgumentNullException.ThrowIfNull(options, nameof(options));

        var sortBuilder = Builders<ConsentAudit>.Sort;

        SortDefinition<ConsentAudit> primarySort = (SortBy, Direction) switch
        {
            (ConsentAuditSortField.Id, SortDirection.Ascending) =>
                sortBuilder.Ascending(x => x.Id),

            (ConsentAuditSortField.Id, SortDirection.Descending) =>
                sortBuilder.Descending(x => x.Id),

            (ConsentAuditSortField.ExternalEntityId, SortDirection.Ascending) =>
                sortBuilder.Ascending(x => x.ExternalEntityId),

            (ConsentAuditSortField.ExternalEntityId, SortDirection.Descending) =>
                sortBuilder.Descending(x => x.ExternalEntityId),

            (ConsentAuditSortField.CreatedAtUtc, SortDirection.Ascending) =>
                sortBuilder.Ascending(x => x.CreatedAtUtc),

            (ConsentAuditSortField.CreatedAtUtc, SortDirection.Descending) =>
                sortBuilder.Descending(x => x.CreatedAtUtc),

            (ConsentAuditSortField.SessionId, SortDirection.Ascending) =>
                sortBuilder.Ascending(x => x.SessionId),

            (ConsentAuditSortField.SessionId, SortDirection.Descending) =>
                sortBuilder.Descending(x => x.SessionId),

            (ConsentAuditSortField.IpAddress, SortDirection.Ascending) =>
                sortBuilder.Ascending(x => x.IpAddress),

            (ConsentAuditSortField.IpAddress, SortDirection.Descending) =>
                sortBuilder.Descending(x => x.IpAddress),

            (ConsentAuditSortField.UserAgent, SortDirection.Ascending) =>
                sortBuilder.Ascending(x => x.UserAgent),

            (ConsentAuditSortField.UserAgent, SortDirection.Descending) =>
                sortBuilder.Descending(x => x.UserAgent),

            (ConsentAuditSortField.RequestPath, SortDirection.Ascending) =>
                sortBuilder.Ascending(x => x.RequestPath),

            (ConsentAuditSortField.RequestPath, SortDirection.Descending) =>
                sortBuilder.Descending(x => x.RequestPath),

            (ConsentAuditSortField.DocumentName, SortDirection.Ascending) =>
                sortBuilder.Ascending(x => x.DocumentName),

            (ConsentAuditSortField.DocumentName, SortDirection.Descending) =>
                sortBuilder.Descending(x => x.DocumentName),

            (ConsentAuditSortField.DocumentHash, SortDirection.Ascending) =>
                sortBuilder.Ascending(x => x.DocumentHash),

            (ConsentAuditSortField.DocumentHash, SortDirection.Descending) =>
                sortBuilder.Descending(x => x.DocumentHash),

            (ConsentAuditSortField.DocumentVersion, SortDirection.Ascending) =>
                sortBuilder.Ascending(x => x.DocumentVersion),

            (ConsentAuditSortField.DocumentVersion, SortDirection.Descending) =>
                sortBuilder.Descending(x => x.DocumentVersion),

            _ => throw new ArgumentOutOfRangeException(
                SortBy.ToString(),
                $"Sorting field {SortBy} is not supported")
        };

        SortDefinition<ConsentAudit> secondarySort = Direction == SortDirection.Ascending
            ? sortBuilder.Ascending(x => x.Id)
            : sortBuilder.Descending(x => x.Id);

        options.Sort = sortBuilder.Combine(primarySort, secondarySort);

        return filter;
    }
}
