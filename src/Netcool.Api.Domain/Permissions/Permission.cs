using Netcool.Core.Entities;

namespace Netcool.Api.Domain.Permissions
{
    public class Permission : FullAuditEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Notes { get; set; }
        public PermissionType Type { get; set; }
        public int MenuId { get; set; }
    }
}