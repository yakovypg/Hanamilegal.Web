using System;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.Pagination;
using Hanamilegal.Web.ApiCommon.Requests;
using Hanamilegal.Web.ApplicationsApi.Services;
using Hanamilegal.Web.Auth.Authorization;
using Hanamilegal.Web.Contracts.Applications;
using Hanamilegal.Web.Contracts.Consents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hanamilegal.Web.ApplicationsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly IConsentsService _consentsService;
    private readonly IApplicationsService _applicationsService;

    public ApplicationsController(
        IConsentsService consentsService,
        IApplicationsService applicationsService)
    {
        ArgumentNullException.ThrowIfNull(consentsService, nameof(consentsService));
        ArgumentNullException.ThrowIfNull(applicationsService, nameof(applicationsService));

        _consentsService = consentsService;
        _applicationsService = applicationsService;
    }

    // POST /api/applications
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApplicationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApplicationDto>> Create(
        [FromBody] CreateApplicationRequestDto dto,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        if (HasConsentValidationErrors(dto))
            return ValidationProblem(ModelState);

        RequestContext requestContext = RequestContext.Create(Request.HttpContext);

        ConsentAuditDto createdConsentAuditDto = await _consentsService.CreatePersonalDataConsentAuditAsync(
            requestContext,
            cancellationToken);

        ApplicationDto createdApplicationDto = await _applicationsService.CreateAsync(dto);

        _ = await _consentsService.AddExternalEntityIdToPersonalDataConsentAuditAsync(
            createdConsentAuditDto.Id,
            createdApplicationDto.Id,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdApplicationDto.Id },
            createdApplicationDto);
    }

    // GET /api/applications/{id}
    [HttpGet("{id:guid}")]
    [Authorize(Policy = nameof(RoleAtLeastRequirement.RoleAtLeastApplicationViewer))]
    [ProducesResponseType(typeof(ApplicationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApplicationDto>> GetById([FromRoute] Guid id)
    {
        ApplicationDto? applicationDto = await _applicationsService.GetByIdAsync(id);

        return applicationDto is null
            ? NotFound()
            : Ok(applicationDto);
    }

    // GET /api/applications/search?...
    [HttpGet("search")]
    [Authorize(Policy = nameof(RoleAtLeastRequirement.RoleAtLeastApplicationViewer))]
    [ProducesResponseType(typeof(PaginationResult<ApplicationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginationResult<ApplicationDto>>> SearchAll(
        [FromQuery] ApplicationSearchRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        PaginationResult<ApplicationDto> foundApplications = await _applicationsService.SearchAllAsync(dto);
        return Ok(foundApplications);
    }

    private bool HasConsentValidationErrors(CreateApplicationRequestDto dto)
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
