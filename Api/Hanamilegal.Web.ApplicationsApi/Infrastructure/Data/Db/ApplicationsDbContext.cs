using System;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.ApplicationsApi.Domain.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Db;

public sealed class ApplicationsDbContext : IdentityDbContext
{
    public ApplicationsDbContext(DbContextOptions<ApplicationsDbContext> options)
        : base(options ?? throw new ArgumentNullException(nameof(options)))
    {
    }

    public DbSet<Application<LegalApplicationType>> LegalApplications { get; set; }
    public DbSet<Application<TechApplicationType>> TechApplications { get; set; }
}
