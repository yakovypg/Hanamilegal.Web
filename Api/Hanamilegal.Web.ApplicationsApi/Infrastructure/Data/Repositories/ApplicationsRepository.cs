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

internal sealed class ApplicationsRepository : IApplicationsRepository
{
    private readonly ApplicationsDbContext _context;
    private readonly ILogger<ApplicationsRepository> _logger;

    public ApplicationsRepository(ApplicationsDbContext context, ILogger<ApplicationsRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _context = context;
        _logger = logger;
    }

    public async Task<long> CountAsync(IEnumerable<IFilter<Application>>? filters = null)
    {
        _logger.LogInformation("Trying to get number of applications");

        IQueryable<Application> query = _context.Applications.ApplyFilters(filters);
        long applicationsCount = await query.LongCountAsync();

        _logger.LogInformation("Received number of applications: {ApplicationsCount}", applicationsCount);

        return applicationsCount;
    }

    public async Task AddAsync(Application application)
    {
        ArgumentNullException.ThrowIfNull(application, nameof(application));

        _logger.LogInformation("Trying to add application");

        await _context.Applications.AddAsync(application);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Application added");
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        _logger.LogInformation("Trying to delete application {ApplicationId}", id);

        Application? foundApplication = await FindByIdAsync(id);

        if (foundApplication is null)
        {
            _logger.LogWarning("Application not found");
            throw new NotFoundException("Application not found");
        }

        _context.Applications.Remove(foundApplication);

        await _context.SaveChangesAsync();

        _logger.LogInformation("Application {ApplicationId} deleted", id);
    }

    public async Task<Application?> FindByIdAsync(Guid id)
    {
        _logger.LogInformation("Trying to find application {ApplicationId}", id);

        Application? foundApplication = await _context.Applications.SingleAsync(t => t.Id == id);

        _logger.LogInformation("Application found: {Found}", foundApplication is not null);

        return foundApplication;
    }

    public async Task<PaginationResult<Application>> FindAsync(
        PaginationFilter<Application> paginationFilter,
        IEnumerable<IFilter<Application>>? filters = null)
    {
        _logger.LogInformation("Trying to find applications");

        IEnumerable<IFilter<Application>> primaryFilters = filters ?? [];
        long totalApplicationsCount = await CountAsync(primaryFilters);

        IEnumerable<IFilter<Application>> allFilters = primaryFilters.Append(paginationFilter);
        IQueryable<Application> query = _context.Applications.ApplyFilters(allFilters);
        List<Application> foundApplications = await query.ToListAsync();

        _logger.LogInformation("Applications found: {Found}", foundApplications.Count > 0);

        return new PaginationResult<Application>()
        {
            PageNumber = paginationFilter.PageNumber,
            PageSize = paginationFilter.PageSize,
            TotalItemsCount = totalApplicationsCount,
            Items = foundApplications
        };
    }
}
