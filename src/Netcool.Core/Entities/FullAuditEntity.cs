using System;

namespace Netcool.Api.Core.Entities
{
    public abstract class FullAuditEntity : FullAuditEntity<int>
    {

    }

    public abstract class FullAuditEntity<TPrimaryKey> : AuditEntity<TPrimaryKey>, IDeleteAudit, ISoftDelete
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeleteTime { get; set; }

        public int? DeleteUserId { get; set; }
    }
}