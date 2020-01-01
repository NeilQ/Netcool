using System.Collections.Generic;

namespace Netcool.Api.Core.Services.Dto
{
    public interface IListResult<T>
    {
        /// <summary>
        /// List of items.
        /// </summary>
        IReadOnlyList<T> Items { get; set; }
    }
}