using Netcool.Core.Entities;

namespace Netcool.Core.User
{
    public class User : FullAuditEntity
    {
        public string Name { get; set; }

        public Gender Gender { get; set; }
    }
}