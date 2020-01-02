using System;
using Netcool.Core.Entities;

namespace Netcool.Core.Services.Dto
{
    /// <summary>
    ///  A shortcut of <see cref="CreateAuditedEntityDto"/> for most used primary key type (<see cref="int"/>).
    /// </summary>
    public abstract class CreateAuditedEntityDto : CreateAuditedEntityDto<int>
    {

    }

    /// <summary>
    /// This class can be inherited for simple Dto objects those are used for entities implement <see cref="ICreateAudit"/> interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of primary key</typeparam>
    public abstract class CreateAuditedEntityDto<TPrimaryKey> : EntityDto<TPrimaryKey>, ICreateAudit
    {
        public DateTime? CreateTime { get; set; }

        public int? CreateUserId { get; set; }
    }
}