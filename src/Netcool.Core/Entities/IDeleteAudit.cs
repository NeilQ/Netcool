using System;

namespace Netcool.Api.Core.Entities
{
    public interface IDeleteAudit
    {
        DateTime? DeleteTime { get; set; }

        int? DeleteUserId { get; set; }
    }
}