using System.ComponentModel.DataAnnotations;
using Netcool.Core.Services.Dto;

namespace Netcool.Core.Organizations
{
    public class OrganizationDto : OrganizationSaveInput
    {
        public OrganizationDto Parent { get; set; }

        public string Path { get; set; }

        public int Depth { get; set; }
    }

    public class OrganizationSaveInput : EntityDto
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(32)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        public int? ParentId { get; set; }
    }
}