using System;
using System.Linq;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.ApplicationsApi.Domain.Enums;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;

internal sealed class ApplicationSearchFilter
{
    internal Guid? Id { get; set; }
    internal DateTimeOffset? FromDateUtc { get; set; }
    internal DateTimeOffset? ToDateUtc { get; set; }
    internal ApplicationType? Type { get; set; }
    internal string? SenderName { get; set; }
    internal string? Organization { get; set; }
    internal string? Email { get; set; }

    internal IQueryable<Application> Apply(IQueryable<Application> applications)
    {
        if (Id is not null)
            applications = applications.Where(t => t.Id == Id);

        if (FromDateUtc is not null)
            applications = applications.Where(t => t.CreatedAtUtc >= FromDateUtc);

        if (ToDateUtc is not null)
            applications = applications.Where(t => t.CreatedAtUtc <= ToDateUtc);

        if (Type is not null)
            applications = applications.Where(t => t.Type == Type);

        if (SenderName is not null)
            applications = applications.Where(t => t.SenderName == SenderName);

        if (Organization is not null)
            applications = applications.Where(t => t.Organization == Organization);

        if (Email is not null)
            applications = applications.Where(t => t.Email == Email);

        return applications;
    }
}
