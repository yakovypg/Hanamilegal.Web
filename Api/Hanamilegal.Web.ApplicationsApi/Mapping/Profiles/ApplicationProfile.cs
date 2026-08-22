using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using Hanamilegal.Web.ApplicationsApi.Contracts;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.ApplicationsApi.Domain.Enums;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;

namespace Hanamilegal.Web.ApplicationsApi.Mapping.Profiles;

internal class ApplicationProfile : Profile
{
    internal ApplicationProfile()
    {
        CreateMap<ApplicationTypeDto, ApplicationType>()
            .ConvertUsingEnumMapping(opt => opt.MapByName())
            .ReverseMap();

        CreateMap<CreateApplicationRequestDto, Application>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAtUtc, o => o.Ignore());
        
        CreateMap<ApplicationSearchRequestDto, ApplicationSearchFilter>()
            .ReverseMap();
        
        CreateMap<Application, ApplicationResponseDto>();
    }
}
