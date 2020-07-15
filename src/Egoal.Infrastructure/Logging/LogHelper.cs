using Egoal.Extensions;
using Egoal.Runtime.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;

namespace Egoal.Logging
{
    public class LogHelper
    {
        private readonly ILogger _logger;

        public LogHelper(ILogger<LogHelper> logger)
        {
            _logger = logger;
        }

        public void LogException(Exception ex)
        {
            LogLevel logLevel = LogLevel.Error;
            if (ex is IHasLogLevel hasLogLevel)
            {
                logLevel = hasLogLevel.LogLevel;
            }

            if (!(ex is TmsException))
            {
                _logger.Log(logLevel, ex, ex.StackTrace);

                if (ex.InnerException != null)
                {
                    _logger.Log(logLevel, ex.InnerException.Message);
                }
            }
            else
            {
                _logger.Log(logLevel, ex.Message);
            }

            LogValidationErrors(ex);
        }

        private void LogValidationErrors(Exception exception)
        {
            if (exception is AggregateException && exception.InnerException != null)
            {
                var aggException = exception as AggregateException;
                if (aggException.InnerException is ValidationException)
                {
                    exception = aggException.InnerException;
                }
            }

            if (!(exception is ValidationException))
            {
                return;
            }

            var validationException = exception as ValidationException;
            if (validationException.ValidationErrors.IsNullOrEmpty())
            {
                return;
            }

            _logger.Log(validationException.LogLevel, $"总共{validationException.ValidationErrors.Count}条验证错误：");
            foreach (var validationResult in validationException.ValidationErrors)
            {
                var memberNames = new StringBuilder();
                if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
                {
                    memberNames.Append(" (").Append(string.Join(", ", validationResult.MemberNames)).Append(")");
                }

                _logger.Log(validationException.LogLevel, $"{validationResult.ErrorMessage}{memberNames.ToString()}");
            }
        }
    }
}
