using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Netcool.HttpProxy
{
    /// <summary>
    ///     Shared Proxy Options
    /// </summary>
    public class SharedProxyOptions
    {
        private int? _webSocketBufferSize;

        /// <summary>
        ///     Message handler used for http message forwarding.
        /// </summary>
        public HttpMessageHandler MessageHandler { get; set; }

        /// <summary>
        ///     Allows to modify HttpRequestMessage before it is sent to the Message Handler.
        /// </summary>
        public Func<HttpRequest, HttpRequestMessage, Task<int>> PrepareRequest { get; set; }

        /// <summary>
        ///     Keep-alive interval for proxied Web Socket connections.
        /// </summary>
        public TimeSpan? WebSocketKeepAliveInterval { get; set; }

        /// <summary>
        ///     Internal send and receive buffers size for proxied Web Socket connections.
        /// </summary>
        public int? WebSocketBufferSize
        {
            get => _webSocketBufferSize;
            set
            {
                if (value.HasValue && value.Value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                _webSocketBufferSize = value;
            }
        }
    }
}