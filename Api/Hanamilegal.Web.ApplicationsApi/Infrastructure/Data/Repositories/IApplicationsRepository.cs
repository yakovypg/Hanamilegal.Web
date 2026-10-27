using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.Filters;
using Hanamilegal.Web.ApiCommon.Pagination;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Repositories;

public interface IApplicationsRepository<TApplicationType>
    where TApplicationType : struct, Enum
{
    Task<long> CountAsync(IEnumerable<IFilter<Application<TApplicationType>>>? filters = null);
    Task AddAsync(Application<TApplicationType> application);
    Task DeleteByIdAsync(Guid id);
    Task<Application<TApplicationType>?> FindByIdAsync(Guid id);

    Task<PaginationResult<Application<TApplicationType>>> FindAsync(
        PaginationFilter<Application<TApplicationType>> paginationFilter,
        IEnumerable<IFilter<Application<TApplicationType>>>? filters = null);
}
