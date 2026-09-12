using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.Filters;
using Hanamilegal.Web.ApiCommon.Pagination;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Repositories;

public interface IApplicationsRepository
{
    Task<long> CountAsync(IEnumerable<IFilter<Application>>? filters = null);
    Task AddAsync(Application application);
    Task DeleteByIdAsync(Guid id);
    Task<Application?> FindByIdAsync(Guid id);

    Task<PaginationResult<Application>> FindAsync(
        PaginationFilter<Application> paginationFilter,
        IEnumerable<IFilter<Application>>? filters = null);
}
