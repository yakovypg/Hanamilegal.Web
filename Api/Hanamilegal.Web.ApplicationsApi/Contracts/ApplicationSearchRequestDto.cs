using System;
using System.ComponentModel.DataAnnotations;
using Hanamilegal.Web.ApiConfiguration.Limits;

namespace Hanamilegal.Web.ApplicationsApi.Contracts;

public sealed class ApplicationSearchRequestDto
{
    public Guid? Id { get; set; }
    public DateTimeOffset? FromDateUtc { get; set; }
    public DateTimeOffset? ToDateUtc { get; set; }
    public ApplicationTypeDto? Type { get; set; }

    [MaxLength(DtoLimits.CommonTextMaxLength)]
    public string? SenderName { get; set; }

    [MaxLength(DtoLimits.CommonTextMaxLength)]
    public string? Organization { get; set; }

    [EmailAddress]
    [MaxLength(DtoLimits.CommonTextMaxLength)]
    public string? Email { get; set; }
}
