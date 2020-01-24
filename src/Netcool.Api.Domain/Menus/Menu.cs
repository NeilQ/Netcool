using Netcool.Core.Entities;

namespace Netcool.Api.Domain.Menus
{
    public class Menu : FullAuditEntity
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public int ParentId { get; set; }

        public string Route { get; set; }

        public string Icon { get; set; }

        public string Blank { get; set; }

        public int Level { get; set; }

        public int Order { get; set; }

        public string Path { get; set; }

        public string Notes { get; set; }
    }
}