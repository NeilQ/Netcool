using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Netcool.Core.WebApi
{
    public class HttpContextClientInfoProvider : IClientInfoProvider
    {
        public string BrowserInfo => GetBrowserInfo();

        public string ClientIpAddress => GetClientIpAddress();

        public string ClientName => GetClientName();

        private readonly ILogger _logger;

        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Creates a new <see cref="HttpContextClientInfoProvider"/>.
        /// </summary>
        public HttpContextClientInfoProvider(IHttpContextAccessor httpContextAccessor,
            ILogger<HttpContextClientInfoProvider> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        protected virtual string GetBrowserInfo()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            return httpContext?.Request?.Headers?["User-Agent"];
        }

        protected virtual string GetClientIpAddress()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;

                var header = httpContext.Request.Headers["X-Real-IP"];
                if (header.Count > 0)
                {
                    return header[0];
                }

                return httpContext.Connection?.RemoteIpAddress?.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString());
            }

            return null;
        }

        protected virtual string GetClientName()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;

                var header = httpContext.Request.Headers["X-Client-Name"];
                if (header.Count > 0)
                {
                    return header[0];
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString());
            }

            return null;
        }
    }
}