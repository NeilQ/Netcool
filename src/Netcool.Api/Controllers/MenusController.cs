using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netcool.Api.Domain.Menus;
using Netcool.Core.AspNetCore.Controllers;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Controllers
{
    [Route("menus")]
    [Authorize]
    public class MenusController : QueryControllerBase<MenuDto, int, PageRequest>
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService service) : base(service)
        {
            _menuService = service;
        }

        [HttpGet("tree")]
        public ActionResult<MenuTreeNode> GetTree()
        {
            var node = _menuService.GetMenuTree();
            return Ok(node);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] MenuDto input)
        {
            input.Id = id;
            Service.Update(input);
            return Ok();
        }
    }
}