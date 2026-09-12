using Hanamilegal.Web.Contracts.Filters;

namespace Hanamilegal.Web.Contracts.Applications;

public sealed class ApplicationSortFilterDto
{
    public ApplicationSortFieldDto SortBy { get; set; }
    public SortDirectionDto Direction { get; set; }
}
