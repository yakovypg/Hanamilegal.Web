using System;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.Pagination;
using Hanamilegal.Web.Contracts.Applications;

namespace Hanamilegal.Web.ApplicationsApi.Services;

public interface IApplicationsService
{
    Task<ApplicationDto> CreateAsync(CreateApplicationRequestDto dto);
    Task<ApplicationDto?> GetByIdAsync(Guid id);
    Task<PaginationResult<ApplicationDto>> SearchAllAsync(ApplicationSearchRequestDto dto);
}
