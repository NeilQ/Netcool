namespace Netcool.Api.Core.Services.Dto
{
    public interface IPagedResult<T> : IListResult<T>
    {
        int Total { get; set; }
    }
}