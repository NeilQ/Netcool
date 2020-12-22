using System.Collections.Generic;
using Netcool.Api.Domain.Users;
using Netcool.Core.Entities;

namespace Netcool.Api.Domain.Roles
{
    public class Role : FullAuditEntity
    {
        public string Name { get; set; }

        public string Notes { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

        public Role()
        {
        }

        public Role(int id, string name, string notes = null)
        {
            Id = id;
            Name = name;
            Notes = notes;
        }
    }
}