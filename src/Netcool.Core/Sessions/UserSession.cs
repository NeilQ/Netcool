using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Netcool.Core.Authorization;

namespace Netcool.Core.Sessions
{
    public class UserSession : IUserSession
    {
        public int UserId { get; set; }

        public int TenantId { get; set; }

        public ClaimsPrincipal ClaimsPrincipal { get; set; }

        public UserSession(IHttpContextAccessor httpContextAccessor)
        {
            ClaimsPrincipal = httpContextAccessor?.HttpContext?.User;
            if (ClaimsPrincipal?.Identity == null || !ClaimsPrincipal.Identity.IsAuthenticated) return;

            var idClaim = ClaimsPrincipal.Claims.FirstOrDefault(x => x.Type == AppClaimTypes.UserId);
            if (!string.IsNullOrEmpty(idClaim?.Value) && int.TryParse(idClaim.Value, out var userId))
            {
                UserId = userId;
            }

            var tenantClaim = ClaimsPrincipal.Claims.FirstOrDefault(x => x.Type == "TenantId");
            if (!string.IsNullOrEmpty(tenantClaim?.Value) && int.TryParse(tenantClaim.Value, out var tenantId))
            {
                TenantId = tenantId;
            }
        }
    }
}