using Egoal.Events.Bus;

namespace Egoal.Orders
{
    public class OrderStatChangingEventData : EventData
    {
        public OrderStat OrderStat { get; set; }
    }
}
