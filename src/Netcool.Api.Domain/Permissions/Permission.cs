using System.Collections.Generic;
using Netcool.Api.Domain.Menus;
using Netcool.Api.Domain.Roles;
using Netcool.Core.Entities;

namespace Netcool.Api.Domain.Permissions
{
    public sealed class Permission : FullAuditEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Notes { get; set; }
        public PermissionType Type { get; set; }
        public int MenuId { get; set; }
        
        public IList<RolePermission> RolePermissions { get; set; }

        public Menu Menu { get; set; }

        public Permission()
        {
        }

        public Permission(int id, string name, string code, PermissionType type, int menuId, string notes = null)
        {
            Id = id;
            Name = name;
            Code = code;
            Type = type;
            MenuId = menuId;
            Notes = notes;
        }
    }
}