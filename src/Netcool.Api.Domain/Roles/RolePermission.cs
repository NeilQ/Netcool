using Netcool.Core.Entities;

namespace Netcool.Api.Domain.Roles
{
    public class RolePermission : CreateAuditEntity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}