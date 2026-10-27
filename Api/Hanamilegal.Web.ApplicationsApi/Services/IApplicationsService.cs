using System;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.Pagination;
using Hanamilegal.Web.Contracts.Applications;

namespace Hanamilegal.Web.ApplicationsApi.Services;

public interface IApplicationsService<TApplicationType>
    where TApplicationType : struct, Enum
{
    Task<ApplicationDto<TApplicationType>> CreateAsync(CreateApplicationRequestDto<TApplicationType> dto);
    Task<ApplicationDto<TApplicationType>?> GetByIdAsync(Guid id);

    Task<PaginationResult<ApplicationDto<TApplicationType>>> SearchAsync(
        ApplicationSearchRequestDto<TApplicationType> dto);
}
