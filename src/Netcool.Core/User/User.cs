using Netcool.Api.Core.Entities;

namespace Netcool.Api.Core.User
{
    public class User : FullAuditEntity
    {
        public string Name { get; set; }

        public Gender Gender { get; set; }
    }
}