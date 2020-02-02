namespace Netcool.Core.WebApi
{
    public interface IClientInfoProvider
    {
        string BrowserInfo { get; }

        string ClientIpAddress { get; }

        string ClientName { get; }
    }
}