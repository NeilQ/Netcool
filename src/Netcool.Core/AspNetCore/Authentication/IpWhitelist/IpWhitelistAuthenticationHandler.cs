using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netcool.Core.Authorization;

namespace Netcool.Core.AspNetCore.Authentication.IpWhitelist
{
    internal class IpWhitelistAuthenticationHandler : AuthenticationHandler<IpWhitelistAuthenticationOptions>
    {
        public IpWhitelistAuthenticationHandler(IOptionsMonitor<IpWhitelistAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // build the claims and put them in "Context"; you need to import the Microsoft.AspNetCore.Authentication package

            if (Options.Enable && Options.Ips is { Count: > 0 })
            {
                if (Request.Host.HasValue &&
                    Options.Ips.Contains(Context.Connection.RemoteIpAddress?.ToString()))
                {
                    return Pass();
                }
            }

            return Task.FromResult(AuthenticateResult.NoResult());
        }

        private Task<AuthenticateResult> Pass()
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(AppClaimTypes.Role, "IPWhitelist")
            }, Options.Schema);
            Context.User = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity),
                new AuthenticationProperties(), Options.Schema);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
