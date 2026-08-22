using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.Contracts.Applications;
using Hanamilegal.Web.InternalApp.Api.Applications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hanamilegal.Web.InternalApp.Pages.Applications;

[Authorize(Policy = "RoleAtLeastApplicationViewer")]
public class IndexModel : PageModel
{
    private readonly ApplicationsApiClient _applicationsApiClient;

    internal IndexModel(ApplicationsApiClient applicationsApiClient)
    {
        ArgumentNullException.ThrowIfNull(applicationsApiClient, nameof(applicationsApiClient));
        _applicationsApiClient = applicationsApiClient;

        Filter = new();
        Applications = [];
    }

    [BindProperty(SupportsGet = true)]
    public ApplicationSearchRequestDto Filter { get; set; }

    public IReadOnlyList<ApplicationResponseDto> Applications { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        if (Filter.FromDateUtc.HasValue &&
            Filter.ToDateUtc.HasValue &&
            Filter.FromDateUtc > Filter.ToDateUtc)
        {
            ModelState.AddModelError(
                nameof(Filter.ToDateUtc),
                "End date cannot be less than start date");

            return;
        }

        if (!ModelState.IsValid)
            return;

        try
        {
            Applications = await _applicationsApiClient.SearchAsync(Filter, cancellationToken);
        }
        catch
        {
            ErrorMessage = "Failed to load applications";
        }
    }
}
