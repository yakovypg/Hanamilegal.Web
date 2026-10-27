using System;
using Hanamilegal.Web.Contracts.Filters;

namespace Hanamilegal.Web.Contracts.Applications;

public sealed class ApplicationSearchRequestDto<TApplicationTypeDto>
    where TApplicationTypeDto : struct, Enum
{
    public ApplicationSearchFilterDto<TApplicationTypeDto>? SearchFilter { get; set; }
    public ApplicationSortFilterDto? SortFilter { get; set; }
    public PaginationFilterDto? PaginationFilter { get; set; }
}
