using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.Pagination;
using Hanamilegal.Web.Auth.Authorization;
using Hanamilegal.Web.Contracts.Consents;
using Hanamilegal.Web.Contracts.Filters;
using Hanamilegal.Web.InternalApp.Api.Consents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Hanamilegal.Web.InternalApp.Pages.Consents;

[Authorize(Policy = nameof(RoleAtLeastRequirement.RoleAtLeastAdmin))]
public class IndexModel : PageModel
{
    private readonly ConsentsApiClient _consentsApiClient;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ConsentsApiClient consentsApiClient, ILogger<IndexModel> logger)
    {
        ArgumentNullException.ThrowIfNull(consentsApiClient, nameof(consentsApiClient));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _consentsApiClient = consentsApiClient;
        _logger = logger;

        SearchFilter = new();
        SortFilter = new();
        PaginationFilter = new();
        PaginationResult = new();
    }

    [BindProperty(SupportsGet = true)]
    public ConsentAuditSearchFilterDto SearchFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    public ConsentAuditSortFilterDto SortFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    public PaginationFilterDto PaginationFilter { get; set; }

    public PaginationResult<ConsentAuditDto> PaginationResult { get; private set; }

    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        ApplyDefaultSearchFilterIfNeeded();
        ApplyDefaultSortFilterIfNeeded();
        ApplyDefaultPaginationFilterIfNeeded();

        if (!IsSearchFilterValid() || !ModelState.IsValid)
            return;

        var request = new ConsentAuditSearchRequestDto()
        {
            SearchFilter = SearchFilter,
            SortFilter = SortFilter,
            PaginationFilter = PaginationFilter
        };

        try
        {
            PaginationResult = await _consentsApiClient.SearchAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load applications: {ErrorMessage}", ex.Message);
            ErrorMessage = "Failed to load applications";
        }
    }

    public string GetPageUrl(int pageNumber)
    {
        Dictionary<string, string?> queryParameters = Request.Query.ToDictionary(
            t => t.Key,
            t => (string?)t.Value.ToString());

        queryParameters[$"{nameof(PaginationFilter)}.{nameof(PaginationFilter.PageNumber)}"] =
            pageNumber.ToString(CultureInfo.InvariantCulture);

        queryParameters[$"{nameof(PaginationFilter)}.{nameof(PaginationFilter.PageSize)}"] =
            PaginationFilter.PageSize.ToString(CultureInfo.InvariantCulture);

        return QueryHelpers.AddQueryString(
            Url.Page("./Index")!,
            queryParameters);
    }

    private bool IsSearchFilterValid()
    {
        if (SearchFilter.FromDateUtc.HasValue &&
            SearchFilter.ToDateUtc.HasValue &&
            SearchFilter.FromDateUtc > SearchFilter.ToDateUtc)
        {
            ModelState.AddModelError(
                nameof(SearchFilter.ToDateUtc),
                "End date cannot be less than start date");

            return false;
        }

        return true;
    }

    private void ApplyDefaultSearchFilterIfNeeded()
    {
        SearchFilter ??= new();
    }

    private void ApplyDefaultSortFilterIfNeeded()
    {
        const string sortByKey = $"{nameof(SortFilter)}.{nameof(SortFilter.SortBy)}";
        const string directionKey = $"{nameof(SortFilter)}.{nameof(SortFilter.Direction)}";

        SortFilter ??= new();

        if (!Request.Query.ContainsKey(sortByKey))
            SortFilter.SortBy = ConsentAuditSortFieldDto.CreatedAtUtc;

        if (!Request.Query.ContainsKey(directionKey))
            SortFilter.Direction = SortDirectionDto.Descending;
    }

    private void ApplyDefaultPaginationFilterIfNeeded()
    {
        const string pageNumberKey = $"{nameof(PaginationFilter)}.{nameof(PaginationFilter.PageNumber)}";
        const string pageSizeKey = $"{nameof(PaginationFilter)}.{nameof(PaginationFilter.PageSize)}";

        PaginationFilter ??= new();

        if (!Request.Query.ContainsKey(pageNumberKey))
            PaginationFilter.PageNumber = 1;

        if (!Request.Query.ContainsKey(pageSizeKey))
            PaginationFilter.PageSize = 10;
    }
}
