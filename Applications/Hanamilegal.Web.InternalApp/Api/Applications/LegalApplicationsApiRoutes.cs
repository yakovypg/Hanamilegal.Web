using Hanamilegal.Web.Contracts.Applications;

namespace Hanamilegal.Web.InternalApp.Api.Applications;

internal sealed class LegalApplicationsApiRoutes : IApplicationsApiRoutes<LegalApplicationTypeDto>
{
    public string Search { get; } = "/api/legal-applications/search";
}
