using System.Security.Claims;

namespace Netcool.Core.Sessions
{
    public interface ICurrentUser
    {
        int UserId { get; set; }

        int TenantId { get; set; }

        ClaimsPrincipal ClaimsPrincipal { get; set; }
    }
}