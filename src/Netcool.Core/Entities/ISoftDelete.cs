namespace Netcool.Core.Entities
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}