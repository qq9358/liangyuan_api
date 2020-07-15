using Egoal.Logging;
using Microsoft.Extensions.Logging;

namespace Egoal
{
    public class ApiException : TmsException, IHasLogLevel
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Error;

        public string OriginalData { get; private set; }

        public ApiException(string message)
            : base(message)
        {
        }

        public ApiException(string message, string originalData)
            : base(message)
        {
            OriginalData = originalData;
        }
    }
}
