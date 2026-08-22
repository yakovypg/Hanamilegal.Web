using System.ComponentModel.DataAnnotations;
using Hanamilegal.Web.ApiConfiguration.Limits;

namespace Hanamilegal.Web.Contracts.Accounts;

public sealed class LoginRequestDto
{
    public LoginRequestDto()
    {
        Email = string.Empty;
        Password = string.Empty;
    }

    [Required]
    [MaxLength(DtoLimits.CommonTextMaxLength)]
    public string Email { get; set; }

    [Required]
    [MaxLength(DtoLimits.CommonTextMaxLength)]
    public string Password { get; set; }
}
