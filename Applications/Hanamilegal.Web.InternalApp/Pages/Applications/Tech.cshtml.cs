using System;
using Hanamilegal.Web.Auth.Authorization;
using Hanamilegal.Web.Contracts.Applications;
using Hanamilegal.Web.InternalApp.Api.Applications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Hanamilegal.Web.InternalApp.Pages.Applications;

[Authorize(Policy = nameof(RoleAtLeastRequirement.RoleAtLeastApplicationViewer))]
public class TechModel : ApplicationsModelBase<TechApplicationTypeDto>
{
    public TechModel(
        ApplicationsApiClient<TechApplicationTypeDto> applicationsApiClient,
        ILogger<TechModel> logger)
        : base(
            nameof(TechModel)[..^"Model".Length],
            applicationsApiClient ?? throw new ArgumentNullException(nameof(applicationsApiClient)),
            logger ?? throw new ArgumentNullException(nameof(logger)))
    {
    }
}
