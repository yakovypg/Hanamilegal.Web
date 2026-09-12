using System;
using System.Linq;
using Hanamilegal.Web.ApiCommon.Filters;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;

public sealed class ApplicationSortFilter : IFilter<Application>
{
    public ApplicationSortField SortBy { get; set; }
    public SortDirection Direction { get; set; }

    public IQueryable<Application> Apply(IQueryable<Application> query)
    {
        ArgumentNullException.ThrowIfNull(query, nameof(query));

        return SortBy switch
        {
            ApplicationSortField.Id =>
                Direction == SortDirection.Ascending
                    ? query.OrderBy(x => x.Id)
                    : query.OrderByDescending(x => x.Id),

            ApplicationSortField.CreatedAtUtc =>
                Direction == SortDirection.Ascending
                    ? query.OrderBy(x => x.CreatedAtUtc).ThenBy(x => x.Id)
                    : query.OrderByDescending(x => x.CreatedAtUtc).ThenByDescending(x => x.Id),

            ApplicationSortField.Type =>
                Direction == SortDirection.Ascending
                    ? query.OrderBy(x => x.Type).ThenBy(x => x.Id)
                    : query.OrderByDescending(x => x.Type).ThenByDescending(x => x.Id),

            ApplicationSortField.SenderName =>
                Direction == SortDirection.Ascending
                    ? query.OrderBy(x => x.SenderName).ThenBy(x => x.Id)
                    : query.OrderByDescending(x => x.SenderName).ThenByDescending(x => x.Id),

            ApplicationSortField.Organization =>
                Direction == SortDirection.Ascending
                    ? query.OrderBy(x => x.Organization).ThenBy(x => x.Id)
                    : query.OrderByDescending(x => x.Organization).ThenByDescending(x => x.Id),

            ApplicationSortField.Email =>
                Direction == SortDirection.Ascending
                    ? query.OrderBy(x => x.Email).ThenBy(x => x.Id)
                    : query.OrderByDescending(x => x.Email).ThenByDescending(x => x.Id),

            _ => throw new ArgumentOutOfRangeException(
                SortBy.ToString(),
                $"Sorting field {SortBy} is not supported")
        };
    }
}
