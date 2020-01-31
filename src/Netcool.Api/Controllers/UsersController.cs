using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Netcool.Api.Domain.Roles;
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

        /// <summary>
        /// Change user password.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("{id}/password/change")]
        public IActionResult ChangePassword(int id, [FromBody] ChangePasswordInput input)
        {
            _userService.ChangePassword(id, input);
            return Ok();
        }


        /// <summary>
        /// Reset user password.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("{id}/password/reset")]
        public IActionResult ResetPassword(int id, [FromBody] ResetPasswordInput input)
        {
            _userService.ResetPassword(id, input);
            return Ok();
        }

        /// <summary>
        /// Get all user roles.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/roles")]
        public ActionResult<IList<Role>> GetUserRoles(int id)
        {
            var roles = _userService.GetUserRoles(id);
            return Ok(roles);
        }

        /// <summary>
        /// Set user roles.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        [HttpPost("{id}/roles")]
        public IActionResult SaveUserRoles(int id, [FromBody] IList<int> roleIds)
        {
            _userService.SaveUserRoles(id, roleIds);
            return Ok();
        }
    }
}