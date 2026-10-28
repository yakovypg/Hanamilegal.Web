using System;
using System.Threading;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.Pagination;
using Hanamilegal.Web.ApplicationsApi.Domain.Enums;
using Hanamilegal.Web.ApplicationsApi.Services;
using Hanamilegal.Web.Auth.Authorization;
using Hanamilegal.Web.Contracts.Applications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hanamilegal.Web.ApplicationsApi.Controllers;

[ApiController]
[Route("api/tech-applications")]
public class TechApplicationsController : ApplicationsControllerBase<TechApplicationType>
{
    public TechApplicationsController(
        IConsentsService consentsService,
        IApplicationsService<TechApplicationType> applicationsService)
        : base(
            consentsService ?? throw new ArgumentNullException(nameof(consentsService)),
            applicationsService ?? throw new ArgumentNullException(nameof(applicationsService)))
    {
    }

    protected override ApplicationProvider ApplicationProvider { get; } = ApplicationProvider.TechApp;

    // POST /api/tech-applications
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApplicationDto<TechApplicationType>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApplicationDto<TechApplicationType>>> Create(
        [FromBody] CreateApplicationRequestDto<TechApplicationType> dto,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));
        return await CreateApplicationAsync(dto, nameof(GetById), cancellationToken);
    }

    // GET /api/tech-applications/{id}
    [HttpGet("{id:guid}")]
    [Authorize(Policy = nameof(RoleAtLeastRequirement.RoleAtLeastApplicationViewer))]
    [ProducesResponseType(typeof(ApplicationDto<TechApplicationType>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApplicationDto<TechApplicationType>>> GetById([FromRoute] Guid id)
    {
        return await GetApplicationByIdAsync(id);
    }

    // GET /api/tech-applications/search?...
    [HttpGet("search")]
    [Authorize(Policy = nameof(RoleAtLeastRequirement.RoleAtLeastApplicationViewer))]
    [ProducesResponseType(typeof(PaginationResult<ApplicationDto<TechApplicationType>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginationResult<ApplicationDto<TechApplicationType>>>> Search(
        [FromQuery] ApplicationSearchRequestDto<TechApplicationType> dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));
        return await SearchApplicationsAsync(dto);
    }
}
