using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hanamilegal.Web.AccountsApi.Contracts;
using Hanamilegal.Web.AccountsApi.Services;
using System;

namespace Hanamilegal.Web.AccountsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountsService _accountsService;

    public AccountsController(IAccountsService accountsService)
    {
        ArgumentNullException.ThrowIfNull(accountsService, nameof(accountsService));
        _accountsService = accountsService;
    }

    // POST /api/accounts/login
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));
    
        LoginResponseDto loginResponseDto = await _accountsService.LoginAsync(dto);
        return Ok(loginResponseDto);
    }
}
