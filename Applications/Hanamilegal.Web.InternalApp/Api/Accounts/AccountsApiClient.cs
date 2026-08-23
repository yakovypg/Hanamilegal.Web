using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.Contracts.Accounts;
using Hanamilegal.Web.InternalApp.Services;

namespace Hanamilegal.Web.InternalApp.Api.Accounts;

public sealed class AccountsApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IJsonContentService _jsonContentService;

    public AccountsApiClient(HttpClient httpClient, IJsonContentService jsonContentService)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(jsonContentService, nameof(jsonContentService));

        _httpClient = httpClient;
        _jsonContentService = jsonContentService;
    }

    public async Task<LoginResponseDto> LoginAsync(
        LoginRequestDto loginData,
        CancellationToken cancellationToken = default)
    {
        string url = AccountsApiRoutes.Login;
        using JsonContent content = JsonContent.Create(loginData);

        HttpResponseMessage response = await _httpClient.PostAsync(url, content, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await _jsonContentService
            .ReadAsync<LoginResponseDto>(response.Content, cancellationToken);
    }
}
