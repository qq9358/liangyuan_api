using Egoal.Events.Bus;

namespace Egoal.Staffs
{
    public class OrderExplainBeginingEventData : EventData
    {
        public string ListNo { get; set; }
        public int ExplainerId { get; set; }
    }
}
