using System;
using System.Runtime.Serialization;

namespace Trsys.CopyTrading.Abstractions
{
    [Serializable]
    public class OrderTextFormatException : Exception
    {
        public OrderTextFormatException()
        {
        }

        public OrderTextFormatException(string message) : base(message)
        {
        }

        public OrderTextFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OrderTextFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}