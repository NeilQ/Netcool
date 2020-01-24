using Netcool.Core.Entities;

namespace Netcool.Api.Domain.Users
{
    public class UserRole : CreateAuditEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public UserRole()
        {
        }

        public UserRole(int userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}