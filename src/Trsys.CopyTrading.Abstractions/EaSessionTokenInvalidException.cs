using System;
using System.Runtime.Serialization;

namespace Trsys.CopyTrading.Abstractions
{
    [Serializable]
    public class EaSessionTokenInvalidException : Exception
    {
        public EaSessionTokenInvalidException()
        {
        }

        public EaSessionTokenInvalidException(string message) : base(message)
        {
        }

        public EaSessionTokenInvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EaSessionTokenInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}