using Hanamilegal.Web.Contracts.Filters;

namespace Hanamilegal.Web.Contracts.Consents;

public sealed class ConsentAuditSortFilterDto
{
    public ConsentAuditSortFieldDto SortBy { get; set; }
    public SortDirectionDto Direction { get; set; }
}
