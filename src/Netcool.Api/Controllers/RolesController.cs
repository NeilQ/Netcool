﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netcool.Api.Domain.Permissions;
using Netcool.Api.Domain.Roles;
using Netcool.Core.AspNetCore.Controllers;

namespace Netcool.Api.Controllers;

[Route("roles")]
[Authorize]
public class RolesController : CrudControllerBase<RoleDto, int, RoleRequest, RoleSaveInput>
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService service) : base(service)
    {
        _roleService = service;
    }

    [HttpGet("{id}/permissions")]
    public ActionResult<IList<PermissionDto>> GetRolePermissions(int id)
    {
        var permissions = _roleService.GetRolePermissions(id);
        return Ok(permissions);
    }

    [HttpPost("{id}/permissions")]
    public async Task<IActionResult> SetRolePermissionsAsync(int id, [FromBody] IList<int> permissionIds)
    {
        await _roleService.SetRolePermissionsAsync(id, permissionIds);
        return Ok();
    }
}
