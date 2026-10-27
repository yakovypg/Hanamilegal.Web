using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.Filters;
using Hanamilegal.Web.ApiCommon.Pagination;
using Hanamilegal.Web.ApiConfiguration.Exceptions;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Db;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Repositories;

internal abstract class ApplicationsRepository<TApplicationType> : IApplicationsRepository<TApplicationType>
    where TApplicationType : struct, Enum
{
    protected ApplicationsRepository(
        ApplicationsDbContext context,
        ILogger<ApplicationsRepository<TApplicationType>> logger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        Context = context;
        Logger = logger;
    }

    protected ApplicationsDbContext Context { get; }
    protected ILogger<ApplicationsRepository<TApplicationType>> Logger { get; }

    protected abstract DbSet<Application<TApplicationType>> ApplicationsSet { get; }

    public async Task<long> CountAsync(IEnumerable<IFilter<Application<TApplicationType>>>? filters = null)
    {
        Logger.LogInformation("Trying to get number of applications");

        IQueryable<Application<TApplicationType>> query = ApplicationsSet.ApplyFilters(filters);
        long applicationsCount = await query.LongCountAsync();

        Logger.LogInformation("Received number of applications: {ApplicationsCount}", applicationsCount);

        return applicationsCount;
    }

    public async Task AddAsync(Application<TApplicationType> application)
    {
        ArgumentNullException.ThrowIfNull(application, nameof(application));

        Logger.LogInformation("Trying to add application");

        await ApplicationsSet.AddAsync(application);
        await Context.SaveChangesAsync();

        Logger.LogInformation("Application added");
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        Logger.LogInformation("Trying to delete application {ApplicationId}", id);

        Application<TApplicationType>? foundApplication = await FindByIdAsync(id);

        if (foundApplication is null)
        {
            Logger.LogWarning("Application not found");
            throw new NotFoundException("Application not found");
        }

        ApplicationsSet.Remove(foundApplication);

        await Context.SaveChangesAsync();

        Logger.LogInformation("Application {ApplicationId} deleted", id);
    }

    public async Task<Application<TApplicationType>?> FindByIdAsync(Guid id)
    {
        Logger.LogInformation("Trying to find application {ApplicationId}", id);

        Application<TApplicationType>? foundApplication = await ApplicationsSet.SingleAsync(t => t.Id == id);

        Logger.LogInformation("Application found: {Found}", foundApplication is not null);

        return foundApplication;
    }

    public async Task<PaginationResult<Application<TApplicationType>>> FindAsync(
        PaginationFilter<Application<TApplicationType>> paginationFilter,
        IEnumerable<IFilter<Application<TApplicationType>>>? filters = null)
    {
        Logger.LogInformation("Trying to find applications");

        IEnumerable<IFilter<Application<TApplicationType>>> primaryFilters = filters ?? [];
        long totalApplicationsCount = await CountAsync(primaryFilters);

        IEnumerable<IFilter<Application<TApplicationType>>> allFilters = primaryFilters.Append(paginationFilter);
        IQueryable<Application<TApplicationType>> query = ApplicationsSet.ApplyFilters(allFilters);
        List<Application<TApplicationType>> foundApplications = await query.ToListAsync();

        Logger.LogInformation("Applications found: {Found}", foundApplications.Count > 0);

        return new PaginationResult<Application<TApplicationType>>()
        {
            PageNumber = paginationFilter.PageNumber,
            PageSize = paginationFilter.PageSize,
            TotalItemsCount = totalApplicationsCount,
            Items = foundApplications
        };
    }
}
