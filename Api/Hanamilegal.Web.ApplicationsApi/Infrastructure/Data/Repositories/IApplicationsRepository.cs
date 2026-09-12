using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.Filters;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Repositories;

public interface IApplicationsRepository
{
    Task<int> CountAsync();
    Task AddAsync(Application application);
    Task DeleteByIdAsync(Guid id);
    Task<Application> FindByIdAsync(Guid id);
    Task<IEnumerable<Application>> FindAsync(IEnumerable<IFilter<Application>>? filters = null);
}
