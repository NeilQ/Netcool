using System.Security.Claims;

namespace Netcool.Core.Sessions
{
    public class NullCurrentUser : ICurrentUser
    {
        public int UserId { get; set; }
        public int? TenantId { get; set; }
        public ClaimsPrincipal ClaimsPrincipal { get; set; }

        public static NullCurrentUser Instance => new NullCurrentUser();
    }
}
