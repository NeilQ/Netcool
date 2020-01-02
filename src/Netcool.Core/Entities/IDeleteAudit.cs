using System;

namespace Netcool.Core.Entities
{
    public interface IDeleteAudit
    {
        DateTime? DeleteTime { get; set; }

        int? DeleteUserId { get; set; }
    }
}