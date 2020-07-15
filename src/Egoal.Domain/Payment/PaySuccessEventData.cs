using Egoal.Events.Bus;

namespace Egoal.Payment
{
    public class PaySuccessEventData : EventData
    {
        public string ListNo { get; set; }
        public int PayTypeId { get; set; }
        public string Attach { get; set; }
    }
}
