namespace Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;

public record struct ApplicationSortParameters(
    ApplicationSortField SortBy,
    SortDirection Direction);
