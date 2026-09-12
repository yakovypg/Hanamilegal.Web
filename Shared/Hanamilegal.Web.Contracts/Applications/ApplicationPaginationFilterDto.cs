using System.ComponentModel.DataAnnotations;

namespace Hanamilegal.Web.Contracts.Applications;

public sealed class ApplicationPaginationFilterDto
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    [Range(1, int.MaxValue)]
    public int PageSize { get; set; } = int.MaxValue;
}
