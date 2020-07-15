using System;
using System.Runtime.Serialization;

namespace Egoal
{
    [Serializable]
    public class TmsException : Exception
    {
        public TmsException()
        {

        }

        public TmsException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        public TmsException(string message)
            : base(message)
        {

        }

        public TmsException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
