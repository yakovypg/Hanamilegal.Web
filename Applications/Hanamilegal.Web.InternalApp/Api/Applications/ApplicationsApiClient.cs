using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.Pagination;
using Hanamilegal.Web.Contracts.Applications;
using Hanamilegal.Web.Contracts.Filters;
using Hanamilegal.Web.InternalApp.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace Hanamilegal.Web.InternalApp.Api.Applications;

public sealed class ApplicationsApiClient<TApplicationTypeDto> : ApiClient
    where TApplicationTypeDto : struct, Enum
{
    private readonly IApplicationsApiRoutes<TApplicationTypeDto> _applicationsApiRoutes;

    public ApplicationsApiClient(
        HttpClient httpClient,
        IJsonContentService jsonContentService,
        IApplicationsApiRoutes<TApplicationTypeDto> applicationsApiRoutes)
        : base(
            httpClient ?? throw new ArgumentNullException(nameof(httpClient)),
            jsonContentService ?? throw new ArgumentNullException(nameof(jsonContentService)))
    {
        ArgumentNullException.ThrowIfNull(applicationsApiRoutes, nameof(applicationsApiRoutes));
        _applicationsApiRoutes = applicationsApiRoutes;
    }

    public async Task<PaginationResult<ApplicationDto<TApplicationTypeDto>>> SearchAsync(
        ApplicationSearchRequestDto<TApplicationTypeDto> request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Dictionary<string, string?> queryParameters = CreateQueryParameters(request);
        string url = QueryHelpers.AddQueryString(_applicationsApiRoutes.Search, queryParameters);

        return await GetAsync<PaginationResult<ApplicationDto<TApplicationTypeDto>>>(url, cancellationToken);
    }

    private static Dictionary<string, string?> CreateQueryParameters(
        ApplicationSearchRequestDto<TApplicationTypeDto> searchRequest)
    {
        ArgumentNullException.ThrowIfNull(searchRequest, nameof(searchRequest));

        var queryParameters = new Dictionary<string, string?>();

        if (searchRequest.SearchFilter is not null)
            AddSearchQueryParameters(queryParameters, searchRequest.SearchFilter);

        if (searchRequest.SortFilter is not null)
            AddSortQueryParameters(queryParameters, searchRequest.SortFilter);

        if (searchRequest.PaginationFilter is not null)
            AddPaginationQueryParameters(queryParameters, searchRequest.PaginationFilter);

        return queryParameters;
    }

    private static void AddSearchQueryParameters(
        Dictionary<string, string?> queryParameters,
        ApplicationSearchFilterDto<TApplicationTypeDto> searchFilter)
    {
        ArgumentNullException.ThrowIfNull(queryParameters, nameof(queryParameters));
        ArgumentNullException.ThrowIfNull(searchFilter, nameof(searchFilter));

        const string searchFilterKey = nameof(ApplicationSearchRequestDto<TApplicationTypeDto>.SearchFilter);

        if (searchFilter.Id.HasValue)
        {
            queryParameters[$"{searchFilterKey}.{nameof(searchFilter.Id)}"] =
                searchFilter.Id.Value.ToString();
        }

        if (searchFilter.FromDateUtc.HasValue)
        {
            queryParameters[$"{searchFilterKey}.{nameof(searchFilter.FromDateUtc)}"] =
                searchFilter.FromDateUtc.Value.ToString("O", CultureInfo.InvariantCulture);
        }

        if (searchFilter.ToDateUtc.HasValue)
        {
            queryParameters[$"{searchFilterKey}.{nameof(searchFilter.ToDateUtc)}"] =
                searchFilter.ToDateUtc.Value.ToString("O", CultureInfo.InvariantCulture);
        }

        if (searchFilter.Type.HasValue)
        {
            queryParameters[$"{searchFilterKey}.{nameof(searchFilter.Type)}"] =
                searchFilter.Type.Value.ToString();
        }

        if (!string.IsNullOrWhiteSpace(searchFilter.SenderName))
        {
            queryParameters[$"{searchFilterKey}.{nameof(searchFilter.SenderName)}"] =
                searchFilter.SenderName;
        }

        if (!string.IsNullOrWhiteSpace(searchFilter.Organization))
        {
            queryParameters[$"{searchFilterKey}.{nameof(searchFilter.Organization)}"] =
                searchFilter.Organization;
        }

        if (!string.IsNullOrWhiteSpace(searchFilter.Email))
        {
            queryParameters[$"{searchFilterKey}.{nameof(searchFilter.Email)}"] =
                searchFilter.Email;
        }
    }

    private static void AddSortQueryParameters(
        Dictionary<string, string?> queryParameters,
        ApplicationSortFilterDto sortFilter)
    {
        ArgumentNullException.ThrowIfNull(queryParameters, nameof(queryParameters));
        ArgumentNullException.ThrowIfNull(sortFilter, nameof(sortFilter));

        const string sortFilterKey = nameof(ApplicationSearchRequestDto<TApplicationTypeDto>.SortFilter);

        queryParameters[$"{sortFilterKey}.{nameof(sortFilter.SortBy)}"] =
            sortFilter.SortBy.ToString();

        queryParameters[$"{sortFilterKey}.{nameof(sortFilter.Direction)}"] =
            sortFilter.Direction.ToString();
    }

    private static void AddPaginationQueryParameters(
        Dictionary<string, string?> queryParameters,
        PaginationFilterDto paginationFilter)
    {
        ArgumentNullException.ThrowIfNull(queryParameters, nameof(queryParameters));
        ArgumentNullException.ThrowIfNull(paginationFilter, nameof(paginationFilter));

        const string paginationFilterKey =
            nameof(ApplicationSearchRequestDto<TApplicationTypeDto>.PaginationFilter);

        queryParameters[$"{paginationFilterKey}.{nameof(paginationFilter.PageNumber)}"] =
            paginationFilter.PageNumber.ToString(CultureInfo.InvariantCulture);

        queryParameters[$"{paginationFilterKey}.{nameof(paginationFilter.PageSize)}"] =
            paginationFilter.PageSize.ToString(CultureInfo.InvariantCulture);
    }
}
