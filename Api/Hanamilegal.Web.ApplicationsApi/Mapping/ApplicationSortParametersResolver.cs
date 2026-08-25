using System;
using AutoMapper;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;
using Hanamilegal.Web.Contracts.Applications;

namespace Hanamilegal.Web.ApplicationsApi.Mapping;

public sealed class ApplicationSortParametersResolver
    : IValueResolver<ApplicationSearchRequestDto, ApplicationSearchFilter, ApplicationSortParameters?>
{
    public ApplicationSortParameters? Resolve(
        ApplicationSearchRequestDto source,
        ApplicationSearchFilter destination,
        ApplicationSortParameters? destMember,
        ResolutionContext context)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(destination, nameof(destination));
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        if (source.SortField is null || source.SortDirection is null)
            return null;

        return new ApplicationSortParameters()
        {
            SortBy = context.Mapper.Map<ApplicationSortField>(source.SortField.Value),
            Direction = context.Mapper.Map<SortDirection>(source.SortDirection.Value)
        };
    }
}
