using Microsoft.AspNetCore.Mvc;
using Netcool.Api.Domain.Authorization;

namespace Netcool.Api.Controllers
{
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AccountController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("authenticate")]
        public ActionResult<LoginResult> Login(LoginInput input)
        {
            var result = _loginService.Login(input);
            return Ok(result);
        }
    }
}