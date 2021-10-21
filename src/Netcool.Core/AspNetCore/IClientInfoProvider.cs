namespace Netcool.Core.AspNetCore
{
    public interface IClientInfoProvider
    {
        string BrowserInfo { get; }

        string ClientIpAddress { get; }

        string ClientName { get; }
    }
}