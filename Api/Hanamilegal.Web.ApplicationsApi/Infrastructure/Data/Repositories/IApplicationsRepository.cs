using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;

namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Repositories;

internal interface IApplicationsRepository
{
    Task AddAsync(Application application);
    Task DeleteByIdAsync(Guid id);
    Task<Application> FindByIdAsync(Guid id);
    Task<IEnumerable<Application>> FindAsync(ApplicationSearchFilter filter);
}
