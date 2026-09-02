using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.Contracts.Accounts;
using Hanamilegal.Web.InternalApp.Services;

namespace Hanamilegal.Web.InternalApp.Api.Accounts;

public sealed class AccountsApiClient : ApiClient
{
    public AccountsApiClient(HttpClient httpClient, IJsonContentService jsonContentService)
        : base(
            httpClient ?? throw new ArgumentNullException(nameof(httpClient)),
            jsonContentService ?? throw new ArgumentNullException(nameof(jsonContentService)))
    {
    }

    public async Task<LoginResponseDto> LoginAsync(
        LoginRequestDto loginData,
        CancellationToken cancellationToken = default)
    {
        string url = AccountsApiRoutes.Login;

        return await PostAsync<LoginRequestDto, LoginResponseDto>(
            url: url,
            content: loginData,
            cancellationToken: cancellationToken);
    }

    public async Task<LoginResponseDto> RefreshAccessTokenAsync(
        RefreshAccessTokenRequestDto refreshAccessTokenData,
        CancellationToken cancellationToken = default)
    {
        string url = AccountsApiRoutes.RefreshAccessToken;

        return await PostAsync<RefreshAccessTokenRequestDto, LoginResponseDto>(
            url: url,
            content: refreshAccessTokenData,
            cancellationToken: cancellationToken);
    }
}
