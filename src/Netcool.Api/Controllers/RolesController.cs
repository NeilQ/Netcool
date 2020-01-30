using Microsoft.AspNetCore.Mvc;
using Netcool.Api.Domain.Roles;
using Netcool.Core.Services.Dto;
using Netcool.Core.WebApi.Controllers;

namespace Netcool.Api.Controllers
{
    [Route("roles")]
    public class RolesController : CrudControllerBase<RoleDto, int, PageRequest, RoleSaveInput>
    {
        public RolesController(IRoleService service) : base(service)
        {
        }
    }
}