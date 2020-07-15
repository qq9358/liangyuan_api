using Egoal.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;

namespace Egoal.UI
{
    [Serializable]
    public class UserFriendlyException : TmsException, IHasLogLevel, IHasErrorCode
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;

        public string Details { get; private set; }
        public int Code { get; set; }

        public UserFriendlyException()
        {

        }

        public UserFriendlyException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        public UserFriendlyException(string message)
            : base(message)
        {
        }

        public UserFriendlyException(string message, LogLevel logLevel)
            : base(message)
        {
            LogLevel = logLevel;
        }

        public UserFriendlyException(int code, string message)
            : this(message)
        {
            Code = code;
        }

        public UserFriendlyException(string message, string details)
            : this(message)
        {
            Details = details;
        }

        public UserFriendlyException(int code, string message, string details)
            : this(message, details)
        {
            Code = code;
        }

        public UserFriendlyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public UserFriendlyException(string message, string details, Exception innerException)
            : this(message, innerException)
        {
            Details = details;
        }
    }
}
