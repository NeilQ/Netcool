using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Menus
{
    public interface IMenuService : ICrudService<MenuDto, int, PageRequest>
    {
        public MenuTreeNode GetMenuTree();
    }
}