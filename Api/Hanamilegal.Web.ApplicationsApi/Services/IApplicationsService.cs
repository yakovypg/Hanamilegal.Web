using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hanamilegal.Web.ApplicationsApi.Contracts;

namespace Hanamilegal.Web.ApplicationsApi.Services;

public interface IApplicationsService
{
    Task<ApplicationResponseDto> CreateAsync(CreateApplicationRequestDto dto);
    Task<ApplicationResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<ApplicationResponseDto>> SearchAllAsync(ApplicationSearchRequestDto dto);
}
