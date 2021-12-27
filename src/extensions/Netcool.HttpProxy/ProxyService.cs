using System;
using System.Net.Http;
using Microsoft.Extensions.Options;

namespace Netcool.HttpProxy
{
    public class ProxyService
    {
        public ProxyService(IOptions<SharedProxyOptions> options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            Options = options.Value;
            Client = new HttpClient(Options.MessageHandler ?? new HttpClientHandler
                                        {AllowAutoRedirect = false, UseCookies = false});
        }

        public SharedProxyOptions Options { get; }
        internal HttpClient Client { get; }
    }
}