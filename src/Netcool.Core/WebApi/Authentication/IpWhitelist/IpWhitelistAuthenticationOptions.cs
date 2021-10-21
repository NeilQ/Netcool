using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;

namespace Netcool.Core.WebApi.Authentication.IpWhitelist
{
    public class IpWhitelistAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string Schema { get; set; } = IpWhitelistAuthenticationDefaults.AuthenticationScheme;

        public bool Enable { get; set; }

        public List<string> Ips { get; set; }
    }
}
