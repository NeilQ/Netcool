namespace Netcool.Api.Core.Entities
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}