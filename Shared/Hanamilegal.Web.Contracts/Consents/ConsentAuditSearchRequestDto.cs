using Hanamilegal.Web.Contracts.Filters;

namespace Hanamilegal.Web.Contracts.Consents;

public sealed class ConsentAuditSearchRequestDto
{
    public ConsentAuditSearchFilterDto? SearchFilter { get; set; }
    public ConsentAuditSortFilterDto? SortFilter { get; set; }
    public PaginationFilterDto? PaginationFilter { get; set; }
}
