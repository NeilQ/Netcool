using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Menus
{
    public class MenuDto : EntityDto
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
    }
}