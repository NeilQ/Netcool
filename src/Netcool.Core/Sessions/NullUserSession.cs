namespace Netcool.Core.Sessions
{
    public class NullUserSession:IUserSession
    {
        public int UserId { get; set; }
        public int TenantId { get; set; }
    }
}