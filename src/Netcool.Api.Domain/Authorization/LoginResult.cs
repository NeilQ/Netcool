using System;
using System.Collections.Generic;
using Netcool.Api.Domain.Users;

namespace Netcool.Api.Domain.Authorization
{
    public class LoginResult
    {
        public UserDto User { get; set; }

        public string AccessToken { get; set; }

        public DateTime? ExpiryAt { get; set; }

        public IList<string> PermissionCodes { get; set; }
    }
}