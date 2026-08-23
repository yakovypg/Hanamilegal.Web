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

    public IQueryable<Application> Apply(IQueryable<Application> applications)
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
