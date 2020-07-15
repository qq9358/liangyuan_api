using Egoal.Events.Bus;
using System;

namespace Egoal.Tickets
{
    public class TicketConsumingEventData : EventData
    {
        public string ListNo { get; set; }
        public int TotalConsumeNum { get; set; }
        public string OrderListNo { get; set; }
        public long? OrderDetailId { get; set; }
        public TicketConsume TicketConsume { get; set; }
    }
}
