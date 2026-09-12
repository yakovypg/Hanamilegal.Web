using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using Hanamilegal.Web.ApiCommon.Filters;
using Hanamilegal.Web.ApiCommon.Pagination;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.ApplicationsApi.Domain.Enums;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;
using Hanamilegal.Web.Contracts.Applications;
using Hanamilegal.Web.Contracts.Filters;

namespace Hanamilegal.Web.ApplicationsApi.Mapping.Profiles;

public sealed class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<ApplicationTypeDto, ApplicationType>()
            .ConvertUsingEnumMapping(opt => opt.MapByName())
            .ReverseMap();

        CreateMap<SortDirectionDto, SortDirection>()
            .ConvertUsingEnumMapping(opt => opt.MapByName())
            .ReverseMap();

        CreateMap<ApplicationSortFieldDto, ApplicationSortField>()
            .ConvertUsingEnumMapping(opt => opt.MapByName())
            .ReverseMap();

        CreateMap<ApplicationSearchFilterDto, ApplicationSearchFilter>();
        CreateMap<ApplicationSortFilterDto, ApplicationSortFilter>();
        CreateMap<PaginationFilterDto, PaginationFilter<Application>>();
        CreateMap<PaginationResult<Application>, PaginationResult<ApplicationDto>>();

        CreateMap<CreateApplicationRequestDto, Application>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAtUtc, o => o.Ignore());

        CreateMap<ApplicationSearchRequestDto, ApplicationSearchFilter>();
        CreateMap<Application, ApplicationDto>();
    }
}
