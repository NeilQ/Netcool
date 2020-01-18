namespace Netcool.Core.Sessions
{
    public interface IUserSession
    {
        int UserId { get; set; }
        
        int TenantId { get; set; }
    }
}