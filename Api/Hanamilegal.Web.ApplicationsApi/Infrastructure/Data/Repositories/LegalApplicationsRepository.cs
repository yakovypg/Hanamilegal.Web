using System;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.ApplicationsApi.Domain.Enums;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Repositories;

internal sealed class LegalApplicationsRepository : ApplicationsRepository<LegalApplicationType>
{
    public LegalApplicationsRepository(
        ApplicationsDbContext context,
        ILogger<ApplicationsRepository<LegalApplicationType>> logger)
        : base(
            context ?? throw new ArgumentNullException(nameof(context)),
            logger ?? throw new ArgumentNullException(nameof(logger)))
    {
    }

    protected override DbSet<Application<LegalApplicationType>> ApplicationsSet => Context.LegalApplications;
}
