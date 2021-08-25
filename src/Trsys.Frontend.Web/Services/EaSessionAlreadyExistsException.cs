using System;
using System.Runtime.Serialization;

namespace Trsys.Frontend.Web.Services
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