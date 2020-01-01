using System;
using System.Runtime.Serialization;

namespace Netcool.Api.Core.Entities
{
    public class NetcoolException : Exception
    {
        public int ErrorCode { get; set; }

        public NetcoolException()
        {

        }


        /// <summary>
        /// Creates a new <see cref="NetcoolException"/> object.
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message">Exception message</param>
        public NetcoolException(int errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Creates a new <see cref="NetcoolException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public NetcoolException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="NetcoolException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public NetcoolException(int errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Creates a new <see cref="NetcoolException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public NetcoolException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}