using Egoal.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Egoal.Logging
{
    public static class LoggerExtensions
    {
        public static void LogException(this ILogger logger, Exception exception)
        {
            LogLevel logLevel = (exception as IHasLogLevel)?.LogLevel ?? LogLevel.Error;

            List<string> messages = new List<string>();
            BuildExceptionMessage(exception, messages);
            foreach (var message in messages)
            {
                logger.Log(logLevel, message);
            }
        }

        private static void BuildExceptionMessage(Exception exception, List<string> messages)
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine();
            messageBuilder.AppendLine($"{exception.GetType().Name}:{exception.Message}");
            if (!(exception is TmsException) && !exception.StackTrace.IsNullOrEmpty())
            {
                messageBuilder.AppendLine("StackTrace:");
                messageBuilder.AppendLine(exception.StackTrace);
            }
            messages.Add(messageBuilder.ToString());

            if (exception.InnerException != null)
            {
                BuildExceptionMessage(exception.InnerException, messages);
            }

            if (exception is AggregateException)
            {
                var aggException = exception as AggregateException;
                if (aggException.InnerExceptions.IsNullOrEmpty())
                {
                    return;
                }

                foreach (var innerException in aggException.InnerExceptions)
                {
                    BuildExceptionMessage(innerException, messages);
                }
            }
        }
    }
}
