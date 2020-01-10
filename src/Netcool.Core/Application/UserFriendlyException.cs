using System;

namespace Netcool.Core.Application
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