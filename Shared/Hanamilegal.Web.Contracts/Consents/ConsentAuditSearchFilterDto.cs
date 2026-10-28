using System;
using System.ComponentModel.DataAnnotations;
using Hanamilegal.Web.ApiConfiguration.Limits;
using Hanamilegal.Web.Contracts.Applications;

namespace Hanamilegal.Web.Contracts.Consents;

public sealed class ConsentAuditSearchFilterDto
{
    public Guid? Id { get; set; }
    public Guid? ExternalEntityId { get; set; }
    public DateTimeOffset? FromDateUtc { get; set; }
    public DateTimeOffset? ToDateUtc { get; set; }
    public ApplicationProviderDto? ApplicationProvider { get; set; }

    [MaxLength(DtoLimits.CommonTextMaxLength)]
    public string? SessionId { get; set; }

    [MaxLength(DtoLimits.CommonTextMaxLength)]
    public string? IpAddress { get; set; }

    [MaxLength(DtoLimits.CommonTextMaxLength)]
    public string? UserAgent { get; set; }

    [MaxLength(DtoLimits.CommonTextMaxLength)]
    public string? RequestPath { get; set; }

    [MaxLength(DtoLimits.CommonTextMaxLength)]
    public string? DocumentName { get; set; }

    [MaxLength(DtoLimits.CommonTextMaxLength)]
    public string? DocumentHash { get; set; }

    [MaxLength(DtoLimits.CommonTextMaxLength)]
    public string? DocumentVersion { get; set; }
}
