using System;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Db;

internal sealed class ApplicationsDbContext : IdentityDbContext
{
    internal ApplicationsDbContext(DbContextOptions<ApplicationsDbContext> options)
        : base(options ?? throw new ArgumentNullException(nameof(options)))
    {
    }

    internal DbSet<Application> Applications { get; set; }
}
