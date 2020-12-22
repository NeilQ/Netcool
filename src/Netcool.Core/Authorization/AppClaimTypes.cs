using System.Security.Claims;

namespace Netcool.Core.Authorization
{
    public static class AppClaimTypes
    {
        public const string UserId = ClaimTypes.NameIdentifier;
        public const string UserName = ClaimTypes.Name;
        public const string Role = ClaimTypes.Role;
    }
}