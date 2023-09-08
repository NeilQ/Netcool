using Microsoft.AspNetCore.Mvc;
using Netcool.Api.Domain.Authorization;

namespace Netcool.Api.Controllers;

[Route("account")]
public class AccountController : ControllerBase
{
    private readonly ILoginService _loginService;

    public AccountController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost("authenticate")]
    public async Task<ActionResult<LoginResult>> LoginAsync(LoginInput input)
    {
        var result = await _loginService.LoginAsync(input);
        return Ok(result);
    }
}
