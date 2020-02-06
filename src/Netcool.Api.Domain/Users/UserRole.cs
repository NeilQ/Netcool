using Netcool.Api.Domain.Roles;
using Netcool.Core.Entities;

namespace Netcool.Api.Domain.Users
{
    public sealed class UserRole : CreateAuditEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public Role Role { get; set; }
        public User User { get; set; }

        public UserRole()
        {
        }

        public UserRole(int userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public UserRole(int id, int userId, int roleId)
        {
            Id = id;
            UserId = userId;
            RoleId = roleId;
        }
    }
}