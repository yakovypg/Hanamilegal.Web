using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using Hanamilegal.Web.ApiCommon.Filters;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.ApplicationsApi.Infrastructure.Filters;
using Hanamilegal.Web.Contracts.Consents;
using Hanamilegal.Web.Contracts.Filters;

namespace Hanamilegal.Web.ApplicationsApi.Mapping.Profiles;

public sealed class ConsentAuditProfile : Profile
{
    public ConsentAuditProfile()
    {
        CreateMap<ConsentAuditSortFieldDto, ConsentAuditSortField>()
            .ConvertUsingEnumMapping(opt => opt.MapByName())
            .ReverseMap();

        CreateMap<ConsentAuditSearchFilterDto, ConsentAuditSearchFilter>();
        CreateMap<ConsentAuditSortFilterDto, ConsentAuditSortFilter>();
        CreateMap<PaginationFilterDto, MongoPaginationFilter<ConsentAudit>>();

        CreateMap<ConsentAuditSearchRequestDto, ApplicationSearchFilter>();
        CreateMap<ConsentAudit, ConsentAuditDto>();
    }
}
