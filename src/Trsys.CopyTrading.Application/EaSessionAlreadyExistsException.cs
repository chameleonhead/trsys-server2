using System;
using System.Runtime.Serialization;

namespace Trsys.CopyTrading.Application
{
    [Serializable]
    public class EaSessionAlreadyExistsException : Exception
    {
        public EaSessionAlreadyExistsException()
        {
        }

        public EaSessionAlreadyExistsException(string message) : base(message)
        {
        }

        public EaSessionAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EaSessionAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}