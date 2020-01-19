using System;

namespace Netcool.Core.WebApi
{
    public class UserFriendlyException : Exception
    {
        public int Code { get; set; }

        public UserFriendlyException(int errorCode, string message)
            : base(message)
        {
            Code = errorCode;
        }
    }
}