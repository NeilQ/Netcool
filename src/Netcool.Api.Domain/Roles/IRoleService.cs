using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Roles
{
    public interface IRoleService : ICrudService<RoleDto, int, PageRequest, RoleSaveInput>
    {
    }
}