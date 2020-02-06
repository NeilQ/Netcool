using System.ComponentModel;
using Netcool.Core.Entities;

namespace Netcool.Api.Domain.Menus
{
    public class Menu : FullAuditEntity
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public int ParentId { get; set; }

        public MenuType Type { get; set; }

        public string Route { get; set; }

        public string Icon { get; set; }

        public int Level { get; set; }

        public int Order { get; set; }

        public string Path { get; set; }

        public string Notes { get; set; }

        public Menu()
        {
        }

        public Menu(int id, string name, string displayName, MenuType type, string route, string icon,
            int level, int order, int parentId, string path)
        {
            Id = id;
            Name = name;
            DisplayName = displayName;
            ParentId = parentId;
            Type = type;
            Route = route;
            Icon = icon;
            Level = level;
            Order = order;
            Path = path;
        }
    }

    public enum MenuType
    {
        [Description("节点")] Node,

        [Description("链接")] Link
    }
}