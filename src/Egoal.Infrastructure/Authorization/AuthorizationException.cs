using Egoal.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;

namespace Egoal.Authorization
{
    [Serializable]
    public class AuthorizationException : TmsException, IHasLogLevel
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;

        public AuthorizationException()
        {
        }

        public AuthorizationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        public AuthorizationException(string message)
            : base(message)
        {
        }

        public AuthorizationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
