using Netcool.Core.Entities;

namespace Netcool.Core
{
    public class UserFriendlyException : NetcoolException
    {
        public UserFriendlyException(int errorCode, string message)
            : base(errorCode, message)
        {
        }

        public UserFriendlyException(string message):base(message)
        {
            
        }
    }
}