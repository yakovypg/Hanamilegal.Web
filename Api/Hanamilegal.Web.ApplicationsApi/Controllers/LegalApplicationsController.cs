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
[Route("api/legal-applications")]
public class LegalApplicationsController : ApplicationsControllerBase<LegalApplicationType>
{
    public LegalApplicationsController(
        IConsentsService consentsService,
        IApplicationsService<LegalApplicationType> applicationsService)
        : base(
            consentsService ?? throw new ArgumentNullException(nameof(consentsService)),
            applicationsService ?? throw new ArgumentNullException(nameof(applicationsService)))
    {
    }

    protected override ApplicationProvider ApplicationProvider { get; } = ApplicationProvider.LegalSupportApp;

    // POST /api/legal-applications
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApplicationDto<LegalApplicationType>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApplicationDto<LegalApplicationType>>> Create(
        [FromBody] CreateApplicationRequestDto<LegalApplicationType> dto,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));
        return await CreateApplicationAsync(dto, nameof(GetById), cancellationToken);
    }

    // GET /api/legal-applications/{id}
    [HttpGet("{id:guid}")]
    [Authorize(Policy = nameof(RoleAtLeastRequirement.RoleAtLeastApplicationViewer))]
    [ProducesResponseType(typeof(ApplicationDto<LegalApplicationType>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApplicationDto<LegalApplicationType>>> GetById([FromRoute] Guid id)
    {
        return await GetApplicationByIdAsync(id);
    }

    // GET /api/legal-applications/search?...
    [HttpGet("search")]
    [Authorize(Policy = nameof(RoleAtLeastRequirement.RoleAtLeastApplicationViewer))]
    [ProducesResponseType(typeof(PaginationResult<ApplicationDto<LegalApplicationType>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginationResult<ApplicationDto<LegalApplicationType>>>> Search(
        [FromQuery] ApplicationSearchRequestDto<LegalApplicationType> dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));
        return await SearchApplicationsAsync(dto);
    }
}
