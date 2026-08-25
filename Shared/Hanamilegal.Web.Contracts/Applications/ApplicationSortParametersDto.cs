namespace Hanamilegal.Web.Contracts.Applications;

public sealed class ApplicationSortParametersDto
{
    public ApplicationSortFieldDto SortBy { get; set; }
    public SortDirectionDto Direction { get; set; }
}
