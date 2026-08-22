using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Db;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;
using Microsoft.EntityFrameworkCore;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Repositories;

internal sealed class ApplicationsRepository : IApplicationsRepository
{
    private readonly ApplicationsDbContext _context;

    public ApplicationsRepository(ApplicationsDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        _context = context;
    }

    public async Task AddAsync(Application application)
    {
        ArgumentNullException.ThrowIfNull(application, nameof(application));

        await _context.Applications.AddAsync(application);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        Application foundApplication = await FindByIdAsync(id);
        _context.Applications.Remove(foundApplication);

        await _context.SaveChangesAsync();
    }

    public async Task<Application> FindByIdAsync(Guid id)
    {
        return await _context.Applications.SingleAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Application>> FindAsync(ApplicationSearchFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter, nameof(filter));

        return await filter
            .Apply(_context.Applications)
            .ToListAsync();
    }
}
