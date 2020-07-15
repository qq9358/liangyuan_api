using Egoal.Events.Bus;

namespace Egoal.Payment
{
    public class PayTimeoutEventData : EventData
    {
        public string ListNo { get; set; }
        public string Attach { get; set; }
    }
}
