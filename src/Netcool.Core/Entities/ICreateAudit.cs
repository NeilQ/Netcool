using System;

namespace Netcool.Api.Core.Entities
{
    public interface ICreateAudit
    {
        DateTime? CreateTime { get; set; }

        int? CreateUserId { get; set; }
    }
}