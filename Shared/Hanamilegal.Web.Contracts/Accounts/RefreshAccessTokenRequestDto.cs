using System.ComponentModel.DataAnnotations;
using Hanamilegal.Web.ApiConfiguration.Limits;

namespace Hanamilegal.Web.Contracts.Accounts;

public sealed class RefreshAccessTokenRequestDto
{
    public RefreshAccessTokenRequestDto()
    {
        RefreshToken = string.Empty;
    }

    [Required]
    [MaxLength(DtoLimits.CommonTextMaxLength)]
    public string RefreshToken { get; set; }
}
