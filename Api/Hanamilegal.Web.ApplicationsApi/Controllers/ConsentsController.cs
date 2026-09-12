using System;
using System.Threading.Tasks;
using Hanamilegal.Web.ApiCommon.Pagination;
using Hanamilegal.Web.ApplicationsApi.Services;
using Hanamilegal.Web.Auth.Authorization;
using Hanamilegal.Web.Contracts.Consents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hanamilegal.Web.ApplicationsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsentsController : ControllerBase
{
    private readonly IConsentsService _consentsService;

    public ConsentsController(IConsentsService consentsService)
    {
        ArgumentNullException.ThrowIfNull(consentsService, nameof(consentsService));
        _consentsService = consentsService;
    }

    // GET /api/consents/{id}
    [HttpGet("{id:guid}")]
    [Authorize(Policy = nameof(RoleAtLeastRequirement.RoleAtLeastAdmin))]
    [ProducesResponseType(typeof(ConsentAuditDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConsentAuditDto>> GetById([FromRoute] Guid id)
    {
        ConsentAuditDto? consentAuditDto = await _consentsService.GetByIdAsync(id);

        return consentAuditDto is null
            ? NotFound()
            : Ok(consentAuditDto);
    }

    // GET /api/consents/search?...
    [HttpGet("search")]
    [Authorize(Policy = nameof(RoleAtLeastRequirement.RoleAtLeastAdmin))]
    [ProducesResponseType(typeof(PaginationResult<ConsentAuditDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginationResult<ConsentAuditDto>>> SearchAll(
        [FromQuery] ConsentAuditSearchRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        PaginationResult<ConsentAuditDto> foundConsentAudits = await _consentsService.SearchAllAsync(dto);
        return Ok(foundConsentAudits);
    }
}
