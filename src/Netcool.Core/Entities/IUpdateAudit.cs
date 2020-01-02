using System;

namespace Netcool.Core.Entities
{
    public interface IUpdateAudit
    {
        DateTime? UpdateTime { get; set; }

        int? UpdateUserId { get; set; }
    }
}