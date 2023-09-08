using System.Collections.Generic;
using System.Threading.Tasks;
using Netcool.Api.Domain.Permissions;
using Netcool.Core.Services;

namespace Netcool.Api.Domain.Roles
{
    public interface IRoleService : ICrudService<RoleDto, int, RoleRequest, RoleSaveInput>
    {
        public IList<PermissionDto> GetRolePermissions(int id);
        
        public Task SetRolePermissionsAsync(int id, IList<int> permissionIds);
    }
}
