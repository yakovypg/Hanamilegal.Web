using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.InternalApp.Services;

namespace Hanamilegal.Web.InternalApp.Api;

public abstract class ApiClient
{
    protected ApiClient(HttpClient httpClient, IJsonContentService jsonContentService)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(jsonContentService, nameof(jsonContentService));

        HttpClient = httpClient;
        JsonContentService = jsonContentService;
    }

    protected HttpClient HttpClient { get; }
    protected IJsonContentService JsonContentService { get; }

    protected async Task<TResponse> PostAsync<TContent, TResponse>(
        string url,
        TContent content,
        CancellationToken cancellationToken = default)
    {
        using JsonContent jsonContent = JsonContent.Create(content);

        HttpResponseMessage response = await HttpClient.PostAsync(url, jsonContent, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await JsonContentService.ReadAsync<TResponse>(response.Content, cancellationToken);
    }

    protected async Task<TResponse> GetAsync<TResponse>(
        string url,
        CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await HttpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await JsonContentService.ReadAsync<TResponse>(response.Content, cancellationToken);
    }
}
