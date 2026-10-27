using Hanamilegal.Web.Contracts.Applications;

namespace Hanamilegal.Web.InternalApp.Api.Applications;

internal sealed class TechApplicationsApiRoutes : IApplicationsApiRoutes<TechApplicationTypeDto>
{
    public string Search { get; } = "/api/tech-applications/search";
}
