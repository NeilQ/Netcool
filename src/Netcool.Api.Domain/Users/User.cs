using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Netcool.Api.Domain.Roles;
using Netcool.Core.Entities;

namespace Netcool.Api.Domain.Users
{
    public class User : FullAuditEntity
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Password { get; set; }

        public Gender Gender { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public IList<UserRole> UserRoles { get; set; }

        [NotMapped]
        public IList<Role> Roles => UserRoles?.Select(t => t.Role).ToList();
    }
}