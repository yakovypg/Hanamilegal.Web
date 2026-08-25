using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.Contracts.Applications;
using Hanamilegal.Web.InternalApp.Api.Applications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Hanamilegal.Web.InternalApp.Pages.Applications;

[Authorize(Policy = "RoleAtLeastApplicationViewer")]
public class IndexModel : PageModel
{
    private readonly ApplicationsApiClient _applicationsApiClient;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ApplicationsApiClient applicationsApiClient, ILogger<IndexModel> logger)
    {
        ArgumentNullException.ThrowIfNull(applicationsApiClient, nameof(applicationsApiClient));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _applicationsApiClient = applicationsApiClient;
        _logger = logger;

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

        if (Filter.SortParameters is null)
            ApplyDefaultSortParameters();

        try
        {
            Applications = await _applicationsApiClient.SearchAsync(Filter, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load applications: {ErrorMessage}", ex.Message);
            ErrorMessage = "Failed to load applications";
        }
    }

    private void ApplyDefaultSortParameters()
    {
        Filter.SortParameters = new()
        {
            SortBy = ApplicationSortFieldDto.CreatedAtUtc,
            Direction = SortDirectionDto.Descending
        };
    }
}
