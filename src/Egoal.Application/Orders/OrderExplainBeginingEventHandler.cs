using Egoal.Dependency;
using Egoal.Events.Bus.Handlers;
using Egoal.Staffs;
using System.Threading.Tasks;

namespace Egoal.Orders
{
    public class OrderExplainBeginingEventHandler : IAsyncEventHandler<OrderExplainBeginingEventData>, IScopedDependency
    {
        private readonly IOrderDomainService _orderDomainService;

        public OrderExplainBeginingEventHandler(IOrderDomainService orderDomainService)
        {
            _orderDomainService = orderDomainService;
        }

        public async Task HandleEventAsync(OrderExplainBeginingEventData eventData)
        {
            await _orderDomainService.BeginExplainAsync(eventData.ListNo, eventData.ExplainerId);
        }
    }
}
