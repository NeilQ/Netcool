using System;

namespace Netcool.Core.Entities
{
    public interface ICreateAudit
    {
        DateTime? CreateTime { get; set; }

        int? CreateUserId { get; set; }
    }
}