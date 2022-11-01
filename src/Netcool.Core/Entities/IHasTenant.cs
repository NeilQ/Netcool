namespace Netcool.Core.Entities
{
    public interface IHasTenant
    {
        int? TenantId { get; set; }
    }
}
