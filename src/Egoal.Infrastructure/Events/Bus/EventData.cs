using System;

namespace Egoal.Events.Bus
{
    [Serializable]
    public abstract class EventData : IEventData
    {
        public DateTime EventTime { get; set; }
        public object EventSource { get; set; }

        protected EventData()
        {
            EventTime = DateTime.Now;
        }
    }
}
