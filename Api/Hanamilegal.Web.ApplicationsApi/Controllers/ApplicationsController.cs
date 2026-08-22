using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hanamilegal.Web.ApplicationsApi.Contracts;
using Hanamilegal.Web.ApplicationsApi.Services;
using Hanamilegal.Web.Auth.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hanamilegal.Web.ApplicationsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationsService _applicationsService;

    public ApplicationsController(IApplicationsService applicationsService)
    {
        ArgumentNullException.ThrowIfNull(applicationsService, nameof(applicationsService));
        _applicationsService = applicationsService;
    }

    // POST /api/applications
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApplicationResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApplicationResponseDto>> Create(
        [FromBody] CreateApplicationRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        ApplicationResponseDto createdApplicationDto =
            await _applicationsService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdApplicationDto.Id },
            createdApplicationDto
        );
    }

    // GET /api/applications/{id}
    [HttpGet("{id:guid}")]
    [Authorize(Policy = nameof(RoleAtLeastRequirement.RoleAtLeastApplicationViewer))]
    [ProducesResponseType(typeof(ApplicationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApplicationResponseDto>> GetById([FromRoute] Guid id)
    {
        ApplicationResponseDto? applicationDto =
            await _applicationsService.GetByIdAsync(id);

        return applicationDto is null
            ? NotFound()
            : Ok(applicationDto);
    }

    // GET /api/applications/search?...
    [HttpGet("search")]
    [Authorize(Policy = nameof(RoleAtLeastRequirement.RoleAtLeastApplicationViewer))]
    [ProducesResponseType(typeof(IEnumerable<ApplicationResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ApplicationResponseDto>>> SearchAll(
        [FromQuery] ApplicationSearchRequestDto filter)
    {
        ArgumentNullException.ThrowIfNull(filter, nameof(filter));

        IEnumerable<ApplicationResponseDto> foundApplications =
            await _applicationsService.SearchAllAsync(filter);

        return Ok(foundApplications);
    }
}
