using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.ApplicationsApi.Contracts;
using Hanamilegal.Web.InternalApp.Api.Applications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hanamilegal.Web.InternalApp.Pages.Applications;

[Authorize(Policy = "RoleAtLeastApplicationViewer")]
public class IndexModel : PageModel
{
    private readonly ApplicationsApiClient _applicationsApiClient;

    public IndexModel(ApplicationsApiClient applicationsApiClient)
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
            // Applications = await _applicationsApiClient.SearchAsync(Filter, cancellationToken);
            Applications =
            [
                new()
                {
                    CreatedAtUtc = new DateTime(2026, 1, 2),
                    Type = ApplicationTypeDto.LegalSupport,
                    SenderName = "Ivan",
                    Organization = "Organization",
                    Email = "Ivan@mail.ru",
                    Text = "Dear Hanamilegal, I send you an application. The text of that application is very long."
                },
                new()
                {
                    CreatedAtUtc = new DateTime(2025, 3, 4),
                    Type = ApplicationTypeDto.SoftwareDevelopment,
                    SenderName = "Maria",
                    Organization = "Organization",
                    Email = "Maria@mail.ru",
                    Text = "Hello! My name is Maria and I want to tell you a lot of questions. First, how are you? What is your name? What the capital of Great Britain? How many months in the year?"
                },
                new(), new(), new()
            ];
        }
        catch
        {
            ErrorMessage = "Failed to load applications";
        }
    }
}
