using System;
using System.Runtime.Serialization;

namespace Trsys.CopyTrading.Abstractions
{
    [Serializable]
    public class PublishOrderFormatException : Exception
    {
        public PublishOrderFormatException()
        {
        }

        public PublishOrderFormatException(string message) : base(message)
        {
        }

        public PublishOrderFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PublishOrderFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}