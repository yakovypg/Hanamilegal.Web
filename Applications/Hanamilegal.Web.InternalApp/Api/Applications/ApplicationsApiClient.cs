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

public sealed class ApplicationsApiClient : ApiClient
{
    public ApplicationsApiClient(HttpClient httpClient, IJsonContentService jsonContentService)
        : base(
            httpClient ?? throw new ArgumentNullException(nameof(httpClient)),
            jsonContentService ?? throw new ArgumentNullException(nameof(jsonContentService)))
    {
    }

    public async Task<IReadOnlyList<ApplicationResponseDto>> SearchAsync(
        ApplicationSearchRequestDto filter,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter, nameof(filter));

        Dictionary<string, string?> queryParameters = CreateQueryParameters(filter);
        string url = QueryHelpers.AddQueryString(ApplicationsApiRoutes.Search, queryParameters);

        return await GetAsync<IReadOnlyList<ApplicationResponseDto>>(url, cancellationToken);
    }

    private static Dictionary<string, string?> CreateQueryParameters(ApplicationSearchRequestDto filter)
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

        if (filter.SortParameters is not null)
        {
            query[$"{nameof(filter.SortParameters)}.{nameof(filter.SortParameters.SortBy)}"] =
                filter.SortParameters.SortBy.ToString();

            query[$"{nameof(filter.SortParameters)}.{nameof(filter.SortParameters.Direction)}"] =
                filter.SortParameters.Direction.ToString();
        }

        return query;
    }
}
