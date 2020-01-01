using System.Collections.Generic;

namespace Netcool.Api.Core.Services.Dto
{
    /// <summary>
    /// Implements <see cref="IListResult{T}" />.
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="Items"/> list</typeparam>
    public class ListResultDto<T> : IListResult<T>
    {
        /// <inheritdoc />
        /// <summary>
        /// List of items.
        /// </summary>
        public IReadOnlyList<T> Items
        {
            get => _items ?? (_items = new List<T>());
            set => _items = value;
        }
        private IReadOnlyList<T> _items;

        /// <summary>
        /// Creates a new <see cref="ListResultDto{T}"/> object.
        /// </summary>
        public ListResultDto()
        {
            
        }

        /// <summary>
        /// Creates a new <see cref="ListResultDto{T}"/> object.
        /// </summary>
        /// <param name="items">List of items</param>
        public ListResultDto(IReadOnlyList<T> items)
        {
            Items = items;
        }
    }
}