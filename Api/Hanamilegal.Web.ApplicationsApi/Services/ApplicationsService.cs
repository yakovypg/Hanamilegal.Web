using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.Filters;
using Hanamilegal.Web.ApiCommon.Pagination;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Repositories;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;
using Hanamilegal.Web.Contracts.Applications;
using Mapster;

namespace Hanamilegal.Web.ApplicationsApi.Services;

public sealed class ApplicationsService<TApplicationType> : IApplicationsService<TApplicationType>
    where TApplicationType : struct, Enum
{
    private readonly IApplicationsRepository<TApplicationType> _applicationRepository;

    public ApplicationsService(IApplicationsRepository<TApplicationType> applicationRepository)
    {
        ArgumentNullException.ThrowIfNull(applicationRepository, nameof(applicationRepository));
        _applicationRepository = applicationRepository;
    }

    public async Task<ApplicationDto<TApplicationType>> CreateAsync(
        CreateApplicationRequestDto<TApplicationType> dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        Application<TApplicationType> application = dto.Adapt<Application<TApplicationType>>();
        await _applicationRepository.AddAsync(application);

        return application.Adapt<ApplicationDto<TApplicationType>>();
    }

    public async Task<ApplicationDto<TApplicationType>?> GetByIdAsync(Guid id)
    {
        Application<TApplicationType>? foundApplication = await _applicationRepository.FindByIdAsync(id);
        return foundApplication?.Adapt<ApplicationDto<TApplicationType>?>();
    }

    public async Task<PaginationResult<ApplicationDto<TApplicationType>>> SearchAsync(
        ApplicationSearchRequestDto<TApplicationType> dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        var searchFilter = dto.SearchFilter?.Adapt<ApplicationSearchFilter<TApplicationType>?>();
        var sortFilter = dto.SortFilter?.Adapt<ApplicationSortFilter<TApplicationType>?>();

        var paginationFilter = dto.PaginationFilter
            ?.Adapt<PaginationFilter<Application<TApplicationType>>?>()
            ?? new();

        IFilter<Application<TApplicationType>>?[] rawFilters = [searchFilter, sortFilter];

        IEnumerable<IFilter<Application<TApplicationType>>> filters =
            rawFilters.OfType<IFilter<Application<TApplicationType>>>();

        PaginationResult<Application<TApplicationType>> foundApplications = await _applicationRepository
            .FindAsync(paginationFilter, filters);

        return foundApplications.Adapt<PaginationResult<ApplicationDto<TApplicationType>>>();
    }
}
