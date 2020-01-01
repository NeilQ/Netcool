using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Netcool.Api.Core.Entities
{
    public abstract class AuditEntity : AuditEntity<int>
    {

    }

    public abstract class AuditEntity<TPrimaryKey> : CreateAuditEntity<TPrimaryKey>, IAudit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdateTime { get; set; }
        public int? UpdateUserId { get; set; }
    }
}