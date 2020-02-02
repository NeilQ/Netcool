using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Users
{
    public class UserLoginAttemptDto : CreateAuditedEntityDto
    {
        public int UserId { get; set; }

        public string LoginName { get; set; }

        public bool Success { get; set; }

        public string ClientIp { get; set; }

        public string ClientName { get; set; }

        public string BrowserInfo { get; set; }
    }
}