using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.Pagination;
using Hanamilegal.Web.Contracts.Consents;
using Hanamilegal.Web.Contracts.Filters;
using Hanamilegal.Web.InternalApp.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace Hanamilegal.Web.InternalApp.Api.Consents;

public sealed class ConsentsApiClient : ApiClient
{
    public ConsentsApiClient(HttpClient httpClient, IJsonContentService jsonContentService)
        : base(
            httpClient ?? throw new ArgumentNullException(nameof(httpClient)),
            jsonContentService ?? throw new ArgumentNullException(nameof(jsonContentService)))
    {
    }

    public async Task<PaginationResult<ConsentAuditDto>> SearchAsync(
        ConsentAuditSearchRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Dictionary<string, string?> queryParameters = CreateQueryParameters(request);
        string url = QueryHelpers.AddQueryString(ConsentsApiRoutes.Search, queryParameters);

        return await GetAsync<PaginationResult<ConsentAuditDto>>(url, cancellationToken);
    }

    private static Dictionary<string, string?> CreateQueryParameters(ConsentAuditSearchRequestDto searchRequest)
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
        ConsentAuditSearchFilterDto searchFilter)
    {
        ArgumentNullException.ThrowIfNull(queryParameters, nameof(queryParameters));
        ArgumentNullException.ThrowIfNull(searchFilter, nameof(searchFilter));

        const string searchFilterKey = nameof(ConsentAuditSearchRequestDto.SearchFilter);

        if (searchFilter.Id.HasValue)
        {
            queryParameters[$"{searchFilterKey}.{nameof(searchFilter.Id)}"] =
                searchFilter.Id.Value.ToString();
        }

        if (searchFilter.ExternalEntityId.HasValue)
        {
            queryParameters[$"{searchFilterKey}.{nameof(searchFilter.ExternalEntityId)}"] =
                searchFilter.ExternalEntityId.Value.ToString();
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

        if (!string.IsNullOrWhiteSpace(searchFilter.SessionId))
        {
            queryParameters[$"{searchFilterKey}.{nameof(searchFilter.SessionId)}"] =
                searchFilter.SessionId;
        }

        if (!string.IsNullOrWhiteSpace(searchFilter.IpAddress))
        {
            queryParameters[$"{searchFilterKey}.{nameof(searchFilter.IpAddress)}"] =
                searchFilter.IpAddress;
        }

        if (!string.IsNullOrWhiteSpace(searchFilter.UserAgent))
        {
            queryParameters[$"{searchFilterKey}.{nameof(searchFilter.UserAgent)}"] =
                searchFilter.UserAgent;
        }

        if (!string.IsNullOrWhiteSpace(searchFilter.RequestPath))
        {
            queryParameters[$"{searchFilterKey}.{nameof(searchFilter.RequestPath)}"] =
                searchFilter.RequestPath;
        }

        if (!string.IsNullOrWhiteSpace(searchFilter.DocumentName))
        {
            queryParameters[$"{searchFilterKey}.{nameof(searchFilter.DocumentName)}"] =
                searchFilter.DocumentName;
        }

        if (!string.IsNullOrWhiteSpace(searchFilter.DocumentHash))
        {
            queryParameters[$"{searchFilterKey}.{nameof(searchFilter.DocumentHash)}"] =
                searchFilter.DocumentHash;
        }

        if (!string.IsNullOrWhiteSpace(searchFilter.DocumentVersion))
        {
            queryParameters[$"{searchFilterKey}.{nameof(searchFilter.DocumentVersion)}"] =
                searchFilter.DocumentVersion;
        }
    }

    private static void AddSortQueryParameters(
        Dictionary<string, string?> queryParameters,
        ConsentAuditSortFilterDto sortFilter)
    {
        ArgumentNullException.ThrowIfNull(queryParameters, nameof(queryParameters));
        ArgumentNullException.ThrowIfNull(sortFilter, nameof(sortFilter));

        const string sortFilterKey = nameof(ConsentAuditSearchRequestDto.SortFilter);

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

        const string paginationFilterKey = nameof(ConsentAuditSearchRequestDto.PaginationFilter);

        queryParameters[$"{paginationFilterKey}.{nameof(paginationFilter.PageNumber)}"] =
            paginationFilter.PageNumber.ToString(CultureInfo.InvariantCulture);

        queryParameters[$"{paginationFilterKey}.{nameof(paginationFilter.PageSize)}"] =
            paginationFilter.PageSize.ToString(CultureInfo.InvariantCulture);
    }
}
