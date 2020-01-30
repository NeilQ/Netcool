using System.ComponentModel.DataAnnotations;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Roles
{
    public class RoleDto : RoleSaveInput
    {
    }

    public class RoleSaveInput : EntityDto
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(31)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Notes { get; set; }
    }
}