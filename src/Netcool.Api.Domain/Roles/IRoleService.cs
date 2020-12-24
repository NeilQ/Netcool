using System.Collections.Generic;
using Netcool.Api.Domain.Permissions;
using Netcool.Core.Services;

namespace Netcool.Api.Domain.Roles
{
    public interface IRoleService : ICrudService<RoleDto, int, RoleRequest, RoleSaveInput>
    {
        public IList<PermissionDto> GetRolePermissions(int id);
        public void SetRolePermissions(int id, IList<int> permissionIds);
    }
}