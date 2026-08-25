using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.Contracts.Applications;
using Hanamilegal.Web.InternalApp.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace Hanamilegal.Web.InternalApp.Api.Applications;

public sealed class ApplicationsApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IJsonContentService _jsonContentService;

    public ApplicationsApiClient(HttpClient httpClient, IJsonContentService jsonContentService)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(jsonContentService, nameof(jsonContentService));

        _httpClient = httpClient;
        _jsonContentService = jsonContentService;
    }

    public async Task<IReadOnlyList<ApplicationResponseDto>> SearchAsync(
        ApplicationSearchRequestDto filter,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter, nameof(filter));

        var query = new Dictionary<string, string?>();

        if (filter.Id.HasValue)
            query[nameof(filter.Id)] = filter.Id.Value.ToString();

        if (filter.FromDateUtc.HasValue)
        {
            query[nameof(filter.FromDateUtc)] =
                filter.FromDateUtc.Value.ToString("O", CultureInfo.InvariantCulture);
        }

        if (filter.ToDateUtc.HasValue)
        {
            query[nameof(filter.ToDateUtc)] =
                filter.ToDateUtc.Value.ToString("O", CultureInfo.InvariantCulture);
        }

        if (filter.Type.HasValue)
            query[nameof(filter.Type)] = filter.Type.Value.ToString();

        if (!string.IsNullOrWhiteSpace(filter.SenderName))
            query[nameof(filter.SenderName)] = filter.SenderName;

        if (!string.IsNullOrWhiteSpace(filter.Organization))
            query[nameof(filter.Organization)] = filter.Organization;

        if (!string.IsNullOrWhiteSpace(filter.Email))
            query[nameof(filter.Email)] = filter.Email;

        if (filter.SortField.HasValue)
            query[nameof(filter.SortField)] = filter.SortField.ToString();

        if (filter.SortDirection.HasValue)
            query[nameof(filter.SortDirection)] = filter.SortDirection.ToString();

        string url = QueryHelpers.AddQueryString(ApplicationsApiRoutes.Search, query);

        HttpResponseMessage response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await _jsonContentService
            .ReadAsync<IReadOnlyList<ApplicationResponseDto>>(response.Content, cancellationToken);
    }
}
