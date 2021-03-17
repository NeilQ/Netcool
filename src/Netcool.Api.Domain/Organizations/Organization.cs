using Netcool.Core.Entities;

namespace Netcool.Core.Organizations
{
    public class Organization : FullAuditEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int? ParentId { get; set; }
        
        public int Depth { get; set; }

        public Organization Parent { get; set; }

        public string Path { get; set; }
    }
}