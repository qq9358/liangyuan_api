using Microsoft.Extensions.Logging;

namespace Egoal.Logging
{
    public interface IHasLogLevel
    {
        LogLevel LogLevel { get; set; }
    }
}
