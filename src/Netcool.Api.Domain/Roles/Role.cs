using Netcool.Core.Entities;

namespace Netcool.Api.Domain.Roles
{
    public class Role : FullAuditEntity
    {
        public string Name { get; set; }

        public string Notes { get; set; }
    }
}