using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.Contracts.Accounts;

namespace Hanamilegal.Web.InternalApp.Api.Accounts;

public sealed class AccountsApiClient
{
    private readonly HttpClient _httpClient;

    public AccountsApiClient(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        _httpClient = httpClient;
    }

    public async Task<LoginResponseDto> LoginAsync(
        LoginRequestDto loginData,
        CancellationToken cancellationToken = default)
    {
        string url = AccountsApiRoutes.Login;
        using JsonContent content = JsonContent.Create(loginData);

        HttpResponseMessage response = await _httpClient.PostAsync(url, content, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<LoginResponseDto>(cancellationToken)
            ?? throw new InvalidDataException("Accounts API returned invalid data");
    }
}
