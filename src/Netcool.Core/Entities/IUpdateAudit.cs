using System;

namespace Netcool.Api.Core.Entities
{
    public interface IUpdateAudit
    {
        DateTime? UpdateTime { get; set; }

        int? UpdateUserId { get; set; }
    }
}