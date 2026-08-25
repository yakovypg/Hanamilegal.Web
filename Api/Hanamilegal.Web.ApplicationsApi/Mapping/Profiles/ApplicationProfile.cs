using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.ApplicationsApi.Domain.Enums;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;
using Hanamilegal.Web.Contracts.Applications;

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

        CreateMap<ApplicationSortParametersDto, ApplicationSortParameters>();

        CreateMap<CreateApplicationRequestDto, Application>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAtUtc, o => o.Ignore());

        CreateMap<ApplicationSearchRequestDto, ApplicationSearchFilter>();
        CreateMap<Application, ApplicationResponseDto>();
    }
}
