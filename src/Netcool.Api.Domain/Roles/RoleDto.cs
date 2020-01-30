using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Roles
{
    public class RoleDto : RoleSaveInput
    {
    }

    public class RoleSaveInput : EntityDto
    {
        public string Name { get; set; }

        public string Notes { get; set; }
    }
}