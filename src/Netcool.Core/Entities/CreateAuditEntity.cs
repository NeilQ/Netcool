using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Netcool.Core.Entities
{
    public abstract class CreateAuditEntity : CreateAuditEntity<int>
    {

    }

    public abstract class CreateAuditEntity<TPrimaryKey> : Entity<TPrimaryKey>, ICreateAudit
    {
        public DateTime? CreateTime { get; set; }

        public int? CreateUserId { get; set; }
    }
}
