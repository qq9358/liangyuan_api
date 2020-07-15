using System;

namespace Egoal.Events.Bus
{
    public interface IEventData
    {
        DateTime EventTime { get; set; }
        object EventSource { get; set; }
    }
}
