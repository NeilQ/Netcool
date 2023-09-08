using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netcool.Api.Domain.Menus;
using Netcool.Api.Domain.Roles;
using Netcool.Api.Domain.Users;
using Netcool.Core.AspNetCore.Controllers;

namespace Netcool.Api.Controllers;

[Route("users")]
[Authorize]
public class UsersController : CrudControllerBase<UserDto, int, UserRequest, UserSaveInput>
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
    public async Task<IActionResult> ChangePasswordAsync(int id, [FromBody] ChangePasswordInput input)
    {
        await _userService.ChangePasswordAsync(id, input);
        return Ok();
    }


    /// <summary>
    /// Reset user password.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("{id}/password/reset")]
    public async Task<IActionResult> ResetPasswordAsync(int id, [FromBody] ResetPasswordInput input)
    {
        await _userService.ResetPasswordAsync(id, input);
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
    public async Task<IActionResult> SaveUserRolesAsync(int id, [FromBody] IList<int> roleIds)
    {
        await _userService.SetUserRolesAsync(id, roleIds);
        return Ok();
    }

    /// <summary>
    /// Get user menus.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/menus/tree")]
    public ActionResult<MenuTreeNode> GetUserMenus(int id)
    {
        var tree = _userService.GetUserMenuTree(id);
        return Ok(tree);
    }
}
