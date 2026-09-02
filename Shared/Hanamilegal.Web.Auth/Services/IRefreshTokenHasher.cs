namespace Hanamilegal.Web.Auth.Services;

public interface IRefreshTokenHasher
{
    string HashRefreshToken(string refreshToken);
}
