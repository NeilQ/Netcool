using System;
using Netcool.Api.Core.Entities;

namespace Netcool.Api.Core.Services.Dto
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