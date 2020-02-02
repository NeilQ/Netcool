using Netcool.Core.Entities;

namespace Netcool.Api.Domain.Users
{
    public class UserLoginAttempt : CreateAuditEntity
    {
        public int UserId { get; set; }

        public string LoginName { get; set; }

        public bool Success { get; set; }

        public string ClientIp { get; set; }

        public string ClientName { get; set; }

        public string BrowserInfo { get; set; }
    }
}