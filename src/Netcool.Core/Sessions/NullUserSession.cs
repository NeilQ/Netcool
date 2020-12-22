using System.Security.Claims;

namespace Netcool.Core.Sessions
{
    public class NullUserSession : IUserSession
    {
        public int UserId { get; set; }
        public int TenantId { get; set; }
        public ClaimsPrincipal ClaimsPrincipal { get; set; }

        public static NullUserSession Instance => new NullUserSession();
    }
}