using System;
using System.Security.Cryptography;
using Hanamilegal.Web.Auth.Options;
using Microsoft.Extensions.Options;

namespace Hanamilegal.Web.Auth.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IOptions<RefreshTokenOptions> _options;

    public RefreshTokenService(IOptions<RefreshTokenOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options, nameof(options));
        _options = options;
    }

    public string CreateRefreshToken()
    {
        Span<byte> randomBytes = stackalloc byte[_options.Value.BytesNumber];
        RandomNumberGenerator.Fill(randomBytes);

        return Convert.ToHexString(randomBytes);
    }
}
