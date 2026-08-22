using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.ApplicationsApi.Contracts;
using Microsoft.AspNetCore.WebUtilities;

namespace Hanamilegal.Web.InternalApp.Api.Applications;

public sealed class ApplicationsApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;

    public ApplicationsApiClient(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        _httpClient = httpClient;
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

        string urlString = QueryHelpers.AddQueryString(ApplicationsApiRoutes.Search, query);
        Uri url = new(urlString);

        HttpResponseMessage response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        IReadOnlyList<ApplicationResponseDto>? foundApplications = await response.Content
            .ReadFromJsonAsync<IReadOnlyList<ApplicationResponseDto>>(JsonOptions, cancellationToken);

        return foundApplications ?? [];
    }
}
