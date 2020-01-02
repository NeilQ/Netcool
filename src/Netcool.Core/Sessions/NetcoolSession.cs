using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Netcool.Core.Sessions
{
    public class NetcoolSession : INetcoolSession
    {
        public int UserId { get; set; }
        
        public NetcoolSession(IHttpContextAccessor httpContextAccessor)
        {
            var u = httpContextAccessor?.HttpContext.User;
            if (u == null) return;
            if (!u.Identity.IsAuthenticated) return;

            var idClaim = u.Claims.FirstOrDefault(x => x.Type == "Id");
            if (string.IsNullOrEmpty(idClaim?.Value) || !int.TryParse(idClaim.Value,out var userId)) return;
            UserId = userId;
        }
    }
}