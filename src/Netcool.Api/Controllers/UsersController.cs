using Microsoft.AspNetCore.Mvc;
using Netcool.Api.Domain.Users;
using Netcool.Core.Services.Dto;
using Netcool.Core.WebApi.Controllers;

namespace Netcool.Api.Controllers
{
    [Route("users")]
    public class UsersController : CrudControllerBase<UserDto, int, PageRequest, UserSaveInput>
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userUserService) :
            base(userUserService)
        {
            _userService = userUserService;
        }

        [HttpPost("{id}/password/change")]
        public IActionResult ChangePassword(int id, [FromBody] ChangePasswordInput input)
        {
            _userService.ChangePassword(id, input);
            return Ok();
        }


        [HttpPost("{id}/password/reset")]
        public IActionResult ResetPassword(int id, [FromBody] ResetPasswordInput input)
        {
            _userService.ResetPassword(id, input);
            return Ok();
        }
    }
}