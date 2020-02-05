using System.Collections.Generic;

namespace Netcool.Api.Domain.Menus
{
    public class MenuTreeNode : MenuDto
    {
        public IList<MenuTreeNode> Children { get; set; } = new List<MenuTreeNode>();
    }
}