using AutoMapper;
using Hanamilegal.Web.ApplicationsApi.Domain.Entities;
using Hanamilegal.Web.Contracts.Applications;

namespace Hanamilegal.Web.ApplicationsApi.Mapping.Profiles;

public sealed class ConsentAuditProfile : Profile
{
    public ConsentAuditProfile()
    {
        CreateMap<ConsentAudit, ConsentAuditDto>();
    }
}
