using Netcool.Api.Domain.Permissions;
using Netcool.Core.Entities;

namespace Netcool.Api.Domain.Roles
{
    public class RolePermission : CreateAuditEntity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        public Role Role { get; set; }
        public Permission Permission { get; set; }

        public RolePermission()
        {
        }

        public RolePermission(int roleId, int permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }
}