using Hanamilegal.Web.Contracts.Filters;

namespace Hanamilegal.Web.Contracts.Applications;

public sealed class ApplicationSearchRequestDto
{
    public ApplicationSearchFilterDto? SearchFilter { get; set; }
    public ApplicationSortFilterDto? SortFilter { get; set; }
    public PaginationFilterDto? PaginationFilter { get; set; }
}
