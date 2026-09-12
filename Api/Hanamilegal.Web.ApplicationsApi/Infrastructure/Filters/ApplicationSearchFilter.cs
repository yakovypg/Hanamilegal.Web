using System;
using System.Linq;
using Hanamilegal.Web.ApiCommon.Filters;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.ApplicationsApi.Domain.Enums;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;

public sealed class ApplicationSearchFilter : IFilter<Application>
{
    public Guid? Id { get; set; }
    public DateTimeOffset? FromDateUtc { get; set; }
    public DateTimeOffset? ToDateUtc { get; set; }
    public ApplicationType? Type { get; set; }
    public string? SenderName { get; set; }
    public string? Organization { get; set; }
    public string? Email { get; set; }

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

        return query;
    }
}
