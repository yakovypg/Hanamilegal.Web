using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hanamilegal.Web.ApiCommon.Filters;
using Hanamilegal.Web.ApiCommon.Pagination;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Repositories;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;
using Hanamilegal.Web.Contracts.Applications;

namespace Hanamilegal.Web.ApplicationsApi.Services;

public sealed class ApplicationsService : IApplicationsService
{
    private readonly IApplicationsRepository _applicationRepository;
    private readonly IMapper _mapper;

    public ApplicationsService(IApplicationsRepository applicationRepository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(applicationRepository, nameof(applicationRepository));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));

        _applicationRepository = applicationRepository;
        _mapper = mapper;
    }

    public async Task<ApplicationDto> CreateAsync(CreateApplicationRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        Application application = _mapper.Map<Application>(dto);
        await _applicationRepository.AddAsync(application);

        return _mapper.Map<ApplicationDto>(application);
    }

    public async Task<ApplicationDto?> GetByIdAsync(Guid id)
    {
        Application? foundApplication = await _applicationRepository.FindByIdAsync(id);
        return _mapper.Map<ApplicationDto?>(foundApplication);
    }

    public async Task<PaginationResult<ApplicationDto>> SearchAllAsync(ApplicationSearchRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        var searchFilter = _mapper.Map<ApplicationSearchFilter?>(dto.SearchFilter);
        var sortFilter = _mapper.Map<ApplicationSortFilter?>(dto.SortFilter);

        var paginationFilter = _mapper.Map<PaginationFilter<Application>?>(dto.PaginationFilter)
            ?? new();

        IFilter<Application>?[] rawFilters = [searchFilter, sortFilter];
        IEnumerable<IFilter<Application>> filters = rawFilters.OfType<IFilter<Application>>();

        PaginationResult<Application> foundApplications = await _applicationRepository
            .FindAsync(paginationFilter, filters);

        return _mapper.Map<PaginationResult<ApplicationDto>>(foundApplications);
    }
}
