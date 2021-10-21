using System;
using Microsoft.AspNetCore.Authentication;

namespace Netcool.Core.WebApi.Authentication.IpWhitelist
{
    public static class IpWhitelistAuthenticationExtensions
    {
        public static AuthenticationBuilder AddIpWhitelist(this AuthenticationBuilder builder,
            Action<IpWhitelistAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<IpWhitelistAuthenticationOptions, IpWhitelistAuthenticationHandler>(
                IpWhitelistAuthenticationDefaults.AuthenticationScheme,
                IpWhitelistAuthenticationDefaults.AuthenticationScheme, configureOptions);
        }
    }
}
