using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Data.Repositories;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;
using Hanamilegal.Web.Contracts.Applications;

namespace Hanamilegal.Web.ApplicationsApi.Services;

internal sealed class ApplicationsService : IApplicationsService
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

    public async Task<IEnumerable<ApplicationResponseDto>> SearchAllAsync(ApplicationSearchRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        ApplicationSearchFilter filter = _mapper.Map<ApplicationSearchFilter>(dto);
        IEnumerable<Application> foundApplications = await _applicationRepository.FindAsync(filter);

        return _mapper.Map<IEnumerable<ApplicationResponseDto>>(foundApplications);
    }
}
