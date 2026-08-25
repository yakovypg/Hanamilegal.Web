using System;
using System.Linq;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.ApplicationsApi.Domain.Enums;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;

public sealed class ApplicationSearchFilter
{
    public Guid? Id { get; set; }
    public DateTimeOffset? FromDateUtc { get; set; }
    public DateTimeOffset? ToDateUtc { get; set; }
    public ApplicationType? Type { get; set; }
    public string? SenderName { get; set; }
    public string? Organization { get; set; }
    public string? Email { get; set; }
    public ApplicationSortParameters? SortParameters { get; set; }

    public IQueryable<Application> Apply(IQueryable<Application> query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        if (Id is not null)
            query = query.Where(t => t.Id == Id);

        if (FromDateUtc is not null)
            query = query.Where(t => t.CreatedAtUtc >= FromDateUtc);

        if (ToDateUtc is not null)
            query = query.Where(t => t.CreatedAtUtc <= ToDateUtc);

        if (Type is not null)
            query = query.Where(t => t.Type == Type);

        if (SenderName is not null)
            query = query.Where(t => t.SenderName == SenderName);

        if (Organization is not null)
            query = query.Where(t => t.Organization == Organization);

        if (Email is not null)
            query = query.Where(t => t.Email == Email);

        if (SortParameters is not null)
            query = ApplySorting(query);

        return query;
    }

    private IQueryable<Application> ApplySorting(IQueryable<Application> query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        if (SortParameters is null)
            return query;

        return SortParameters.Value.SortBy switch
        {
            ApplicationSortField.Id =>
                SortParameters.Value.Direction == SortDirection.Ascending
                    ? query.OrderBy(x => x.Id)
                    : query.OrderByDescending(x => x.Id),

            ApplicationSortField.CreatedAtUtc =>
                SortParameters.Value.Direction == SortDirection.Ascending
                    ? query.OrderBy(x => x.CreatedAtUtc).ThenBy(x => x.Id)
                    : query.OrderByDescending(x => x.CreatedAtUtc).ThenByDescending(x => x.Id),

            ApplicationSortField.Type =>
                SortParameters.Value.Direction == SortDirection.Ascending
                    ? query.OrderBy(x => x.Type).ThenBy(x => x.Id)
                    : query.OrderByDescending(x => x.Type).ThenByDescending(x => x.Id),

            ApplicationSortField.SenderName =>
                SortParameters.Value.Direction == SortDirection.Ascending
                    ? query.OrderBy(x => x.SenderName).ThenBy(x => x.Id)
                    : query.OrderByDescending(x => x.SenderName).ThenByDescending(x => x.Id),

            ApplicationSortField.Organization =>
                SortParameters.Value.Direction == SortDirection.Ascending
                    ? query.OrderBy(x => x.Organization).ThenBy(x => x.Id)
                    : query.OrderByDescending(x => x.Organization).ThenByDescending(x => x.Id),

            ApplicationSortField.Email =>
                SortParameters.Value.Direction == SortDirection.Ascending
                    ? query.OrderBy(x => x.Email).ThenBy(x => x.Id)
                    : query.OrderByDescending(x => x.Email).ThenByDescending(x => x.Id),

            _ => throw new ArgumentOutOfRangeException(
                SortParameters.Value.SortBy.ToString(),
                $"Sorting field {SortParameters.Value.SortBy} is not supported")
        };
    }
}
