using System.Linq;
using Microsoft.AspNetCore.Http;
using Netcool.Core.Authorization;

namespace Netcool.Core.Sessions
{
    public class UserSession : IUserSession
    {
        public int UserId { get; set; }

        public int TenantId { get; set; }

        public UserSession(IHttpContextAccessor httpContextAccessor)
        {
            var u = httpContextAccessor?.HttpContext?.User;
            if (u == null) return;
            if (!u.Identity.IsAuthenticated) return;

            var idClaim = u.Claims.FirstOrDefault(x => x.Type == AppClaimTypes.UserId);
            if (!string.IsNullOrEmpty(idClaim?.Value) && int.TryParse(idClaim.Value, out var userId))
            {
                UserId = userId;
            }

            var tenantClaim = u.Claims.FirstOrDefault(x => x.Type == "TenantId");
            if (!string.IsNullOrEmpty(tenantClaim?.Value) && int.TryParse(tenantClaim.Value, out var tenantId))
            {
                TenantId = tenantId;
            }
        }
    }
}