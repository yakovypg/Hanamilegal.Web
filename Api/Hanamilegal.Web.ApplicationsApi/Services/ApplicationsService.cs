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

    public async Task<ApplicationResponseDto> CreateAsync(CreateApplicationRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        Application application = _mapper.Map<Application>(dto);
        await _applicationRepository.AddAsync(application);

        return _mapper.Map<ApplicationResponseDto>(application);
    }

    public async Task<ApplicationResponseDto?> GetByIdAsync(Guid id)
    {
        Application foundApplication = await _applicationRepository.FindByIdAsync(id);
        return _mapper.Map<ApplicationResponseDto>(foundApplication);
    }

    public async Task<PaginationResult<ApplicationResponseDto>> SearchAllAsync(ApplicationSearchRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        var searchFilter = _mapper.Map<ApplicationSearchFilter?>(dto.SearchFilter);
        var sortFilter = _mapper.Map<ApplicationSortFilter?>(dto.SortFilter);
        var paginationFilter = _mapper.Map<PaginationFilter<Application>?>(dto.PaginationFilter);

        IFilter<Application>?[] rawFilters = [searchFilter, sortFilter, paginationFilter];
        IEnumerable<IFilter<Application>> filters = rawFilters.Where(t => t is not null)!;

        IEnumerable<Application> foundApplications = await _applicationRepository.FindAsync(filters);
        var foundApplicationsDto = _mapper.Map<IReadOnlyList<ApplicationResponseDto>>(foundApplications);

        int totalApplicationsCount = await _applicationRepository.CountAsync();

        return new PaginationResult<ApplicationResponseDto>()
        {
            PageNumber = paginationFilter?.PageNumber ?? 1,
            PageSize = paginationFilter?.PageSize ?? int.MaxValue,
            TotalItemsCount = totalApplicationsCount,
            Items = foundApplicationsDto
        };
    }
}
