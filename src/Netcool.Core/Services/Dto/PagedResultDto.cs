using System.Collections.Generic;

namespace Netcool.Core.Services.Dto
{
    public class PagedResultDto<T> : ListResultDto<T>, IPagedResult<T>
    {

        public int Total { get; set; }

        /// <summary>
        /// Creates a new <see cref="PagedResultDto{T}"/> object.
        /// </summary>
        public PagedResultDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="PagedResultDto{T}"/> object.
        /// </summary>
        /// <param name="total">Total count of Items</param>
        /// <param name="items">List of items in current page</param>
        public PagedResultDto(int total, IReadOnlyList<T> items)
            : base(items)
        {
            Total = total;
        }
    }
}