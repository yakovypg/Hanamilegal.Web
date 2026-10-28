using System;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.Pagination;
using Hanamilegal.Web.ApiCommon.Requests;
using Hanamilegal.Web.ApplicationsApi.Domain.Enums;
using Hanamilegal.Web.ApplicationsApi.Services;
using Hanamilegal.Web.Contracts.Applications;
using Hanamilegal.Web.Contracts.Consents;
using Microsoft.AspNetCore.Mvc;

namespace Hanamilegal.Web.ApplicationsApi.Controllers;

public abstract class ApplicationsControllerBase<TApplicationType> : ControllerBase
    where TApplicationType : struct, Enum
{
    private readonly IConsentsService _consentsService;
    private readonly IApplicationsService<TApplicationType> _applicationsService;

    protected ApplicationsControllerBase(
        IConsentsService consentsService,
        IApplicationsService<TApplicationType> applicationsService)
    {
        ArgumentNullException.ThrowIfNull(consentsService, nameof(consentsService));
        ArgumentNullException.ThrowIfNull(applicationsService, nameof(applicationsService));

        _consentsService = consentsService;
        _applicationsService = applicationsService;
    }

    protected abstract ApplicationProvider ApplicationProvider { get; }

    protected async Task<ActionResult<ApplicationDto<TApplicationType>>> CreateApplicationAsync(
        CreateApplicationRequestDto<TApplicationType> dto,
        string createdAtActionName,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));
        ArgumentException.ThrowIfNullOrWhiteSpace(createdAtActionName, nameof(createdAtActionName));

        if (HasConsentValidationErrors(dto))
            return ValidationProblem(ModelState);

        RequestContext requestContext = RequestContext.Create(Request.HttpContext);

        ConsentAuditDto createdConsentAuditDto = await _consentsService.CreatePersonalDataConsentAuditAsync(
            ApplicationProvider,
            requestContext,
            cancellationToken);

        ApplicationDto<TApplicationType> createdApplicationDto =
            await _applicationsService.CreateAsync(dto);

        _ = await _consentsService.AddExternalEntityIdToPersonalDataConsentAuditAsync(
            createdConsentAuditDto.Id,
            createdApplicationDto.Id,
            cancellationToken);

        return CreatedAtAction(
            createdAtActionName,
            new { id = createdApplicationDto.Id },
            createdApplicationDto);
    }

    protected async Task<ActionResult<ApplicationDto<TApplicationType>>> GetApplicationByIdAsync(Guid id)
    {
        ApplicationDto<TApplicationType>? applicationDto = await _applicationsService.GetByIdAsync(id);

        return applicationDto is null
            ? NotFound()
            : Ok(applicationDto);
    }

    protected async Task<ActionResult<PaginationResult<ApplicationDto<TApplicationType>>>> SearchApplicationsAsync(
        ApplicationSearchRequestDto<TApplicationType> dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        PaginationResult<ApplicationDto<TApplicationType>> foundApplications =
            await _applicationsService.SearchAsync(dto);

        return Ok(foundApplications);
    }

    private bool HasConsentValidationErrors(CreateApplicationRequestDto<TApplicationType> dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        bool hasErrors = false;

        if (!dto.HasPersonalDataProcessingConsent)
        {
            ModelState.AddModelError(
                nameof(dto.HasPersonalDataProcessingConsent),
                "Consent to the processing of personal data has not been given");

            hasErrors = true;
        }

        return hasErrors;
    }
}
