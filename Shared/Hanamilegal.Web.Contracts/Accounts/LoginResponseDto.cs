namespace Hanamilegal.Web.Contracts.Accounts;

public sealed class LoginResponseDto
{
    public LoginResponseDto()
    {
        AccessToken = string.Empty;
    }

    public string AccessToken { get; set; }
}
