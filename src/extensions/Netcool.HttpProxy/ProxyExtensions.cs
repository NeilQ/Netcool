using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Netcool.HttpProxy
{
    public static class ProxyExtensions
    {
        /// <summary>
        /// Branches the request pipeline based on matches of the given request path.
        /// If the request path starts with the given path, proxy forwarding the request to the server specified by base url.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="pathMatch">The request path to match</param>
        /// <param name="baseUrl">Destination base url</param>
        /// <returns></returns>
        public static IApplicationBuilder MapProxy(this IApplicationBuilder app, string pathMatch, string baseUrl)
        {
            return app.Map(pathMatch, appBuilder => { appBuilder.RunProxy(new Uri(baseUrl)); });
        }

        public static IEndpointConventionBuilder MapProxy(this IEndpointRouteBuilder endpoints, string pattern,
            Func<RouteValueDictionary, string> routeValueDelegate)
        {
            if (endpoints == null) throw new ArgumentNullException(nameof(endpoints));

            return endpoints.Map(pattern, context =>
            {
                var url = routeValueDelegate.Invoke(context.GetRouteData()?.Values ?? new RouteValueDictionary());
                var targetUri = new Uri(url);
                var uri = new Uri(UriHelper.BuildAbsolute(targetUri.Scheme, new HostString(targetUri.Authority),
                    targetUri.AbsolutePath, null, context.Request.QueryString.Add(new QueryString(targetUri.Query))));
                return context.ProxyRequest(uri);
            });
        }

        /// <summary>
        /// Branches the request pipeline based on matches of the given request path.
        /// If the request path starts with the given path, proxy forwarding the request to the server specified by base url.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="pathMatch">The request path to match</param>
        /// <param name="baseUri">Destination base uri</param>
        /// <returns></returns>
        public static IApplicationBuilder MapProxy(this IApplicationBuilder app, string pathMatch, Uri baseUri)
        {
            return app.Map(pathMatch, appBuilder => { appBuilder.RunProxy(baseUri); });
        }

        /// <summary>
        /// Runs proxy forwarding requests to the server specified by base url.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="baseUrl">Destination base url</param>
        public static void RunProxy(this IApplicationBuilder app, string baseUrl)
        {
            RunProxy(app, new Uri(baseUrl));
        }

        /// <summary>
        /// Runs proxy forwarding requests to the server specified by base uri.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="baseUri">Destination base uri</param>
        public static void RunProxy(this IApplicationBuilder app, Uri baseUri)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (baseUri == null) throw new ArgumentNullException(nameof(baseUri));

            var options = new ProxyOptions
            {
                Scheme = baseUri.Scheme,
                Host = new HostString(baseUri.Authority),
                PathBase = baseUri.AbsolutePath,
                AppendQuery = new QueryString(baseUri.Query)
            };
            RunProxy(app, options);
        }

        /// <summary>
        /// Runs proxy forwarding requests to the server specified by options.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options">Proxy options</param>
        public static void RunProxy(this IApplicationBuilder app, ProxyOptions options)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (options == null) throw new ArgumentNullException(nameof(options));

            app.UseMiddleware<ProxyMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Forwards current request to the specified destination uri.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationUri">Destination Uri</param>
        public static async Task ProxyRequest(this HttpContext context, Uri destinationUri)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (destinationUri == null) throw new ArgumentNullException(nameof(destinationUri));

            if (context.WebSockets.IsWebSocketRequest)
            {
                await context.AcceptProxyWebSocketRequest(destinationUri.ToWebSocketScheme());
            }
            else
            {
                var proxyService = context.RequestServices.GetRequiredService<ProxyService>();

                using (var requestMessage = context.CreateProxyHttpRequest(destinationUri))
                {
                    var prepareRequestHandler = proxyService.Options.PrepareRequest;
                    if (prepareRequestHandler != null)
                    {
                        var res = await prepareRequestHandler(context.Request, requestMessage);
                        if (res >= 100)
                        {
                            context.Response.StatusCode = res;
                            await context.Response.WriteAsync("AccessDenied", Encoding.UTF8);
                            return;
                        }
                    }

                    using (var responseMessage = await context.SendProxyHttpRequest(requestMessage))
                    {
                        await context.CopyProxyHttpResponse(responseMessage);
                    }
                }
            }
        }
    }
}
