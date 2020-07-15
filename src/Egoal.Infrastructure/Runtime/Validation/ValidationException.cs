using Egoal.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Egoal.Runtime.Validation
{
    [Serializable]
    public class ValidationException : TmsException, IHasLogLevel
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;

        public IList<ValidationResult> ValidationErrors { get; set; }

        public ValidationException()
        {
            ValidationErrors = new List<ValidationResult>();
        }

        public ValidationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
            ValidationErrors = new List<ValidationResult>();
        }

        public ValidationException(string message)
            : base(message)
        {
            ValidationErrors = new List<ValidationResult>();
        }

        public ValidationException(string message, IList<ValidationResult> validationErrors)
            : base(message)
        {
            ValidationErrors = validationErrors;
        }

        public ValidationException(string message, Exception innerException)
        {
            ValidationErrors = new List<ValidationResult>();
        }
    }
}
