using System;
using System.Security.Cryptography;
using System.Text;

namespace Hanamilegal.Web.Auth.Services;

public class Sha256RefreshTokenHasher : IRefreshTokenHasher
{
    public string HashRefreshToken(string refreshToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));

        byte[] refreshTokenBytes = Encoding.UTF8.GetBytes(refreshToken);
        byte[] sha256Bytes = SHA256.HashData(refreshTokenBytes);

        return Convert.ToHexString(sha256Bytes);
    }
}
