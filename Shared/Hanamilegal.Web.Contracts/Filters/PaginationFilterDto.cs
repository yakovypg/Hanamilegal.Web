using System.ComponentModel.DataAnnotations;

namespace Hanamilegal.Web.Contracts.Filters;

public sealed class PaginationFilterDto
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    [Range(1, int.MaxValue)]
    public int PageSize { get; set; } = int.MaxValue;
}
