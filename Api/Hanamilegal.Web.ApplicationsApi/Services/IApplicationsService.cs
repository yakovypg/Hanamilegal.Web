using System;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.Pagination;
using Hanamilegal.Web.Contracts.Applications;

namespace Hanamilegal.Web.ApplicationsApi.Services;

public interface IApplicationsService
{
    Task<ApplicationResponseDto> CreateAsync(CreateApplicationRequestDto dto);
    Task<ApplicationResponseDto?> GetByIdAsync(Guid id);
    Task<PaginationResult<ApplicationResponseDto>> SearchAllAsync(ApplicationSearchRequestDto dto);
}
