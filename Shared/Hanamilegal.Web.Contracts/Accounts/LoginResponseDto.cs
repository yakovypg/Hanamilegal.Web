using System;

namespace Hanamilegal.Web.Contracts.Accounts;

public sealed class LoginResponseDto
{
    public LoginResponseDto()
    {
        AccessToken = string.Empty;
    }

    public string AccessToken { get; set; }
    public DateTimeOffset ExpireDateUtc { get; set; }
}
