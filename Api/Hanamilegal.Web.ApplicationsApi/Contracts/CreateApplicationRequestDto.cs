using System.ComponentModel.DataAnnotations;
using Hanamilegal.Web.ApiConfiguration.Limits;

namespace Hanamilegal.Web.ApplicationsApi.Contracts;

public sealed class CreateApplicationRequestDto
{
    public CreateApplicationRequestDto()
    {
        SenderName = string.Empty;
        Organization = string.Empty;
        Email = string.Empty;
        Text = string.Empty;
    }

    [Required]
    public ApplicationTypeDto Type { get; set; }

    [Required, MaxLength(DtoLimits.CommonTextMaxLength)]
    public string SenderName { get; set; }

    [Required, MaxLength(DtoLimits.CommonTextMaxLength)]
    public string Organization { get; set; }

    [Required, EmailAddress, MaxLength(DtoLimits.CommonTextMaxLength)]
    public string Email { get; set; }

    [Required, MaxLength(DtoLimits.LongTextMaxLength)]
    public string Text { get; set; }
}
