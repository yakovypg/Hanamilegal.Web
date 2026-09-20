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

public sealed class ApplicationsService : IApplicationsService
{
    private readonly IApplicationsRepository _applicationRepository;

    public ApplicationsService(IApplicationsRepository applicationRepository)
    {
        ArgumentNullException.ThrowIfNull(applicationRepository, nameof(applicationRepository));
        _applicationRepository = applicationRepository;
    }

    public async Task<ApplicationDto> CreateAsync(CreateApplicationRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        Application application = dto.Adapt<Application>();
        await _applicationRepository.AddAsync(application);

        return application.Adapt<ApplicationDto>();
    }

    public async Task<ApplicationDto?> GetByIdAsync(Guid id)
    {
        Application? foundApplication = await _applicationRepository.FindByIdAsync(id);
        return foundApplication?.Adapt<ApplicationDto?>();
    }

    public async Task<PaginationResult<ApplicationDto>> SearchAllAsync(ApplicationSearchRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        var searchFilter = dto.SearchFilter?.Adapt<ApplicationSearchFilter?>();
        var sortFilter = dto.SortFilter?.Adapt<ApplicationSortFilter?>();

        var paginationFilter = dto.PaginationFilter?.Adapt<PaginationFilter<Application>?>()
            ?? new();

        IFilter<Application>?[] rawFilters = [searchFilter, sortFilter];
        IEnumerable<IFilter<Application>> filters = rawFilters.OfType<IFilter<Application>>();

        PaginationResult<Application> foundApplications = await _applicationRepository
            .FindAsync(paginationFilter, filters);

        return foundApplications.Adapt<PaginationResult<ApplicationDto>>();
    }
}
