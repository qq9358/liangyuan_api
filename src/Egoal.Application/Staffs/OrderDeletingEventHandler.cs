using Egoal.Dependency;
using Egoal.Events.Bus.Entities;
using Egoal.Events.Bus.Handlers;
using Egoal.Orders;
using System.Threading.Tasks;

namespace Egoal.Staffs
{
    public class OrderDeletingEventHandler : IAsyncEventHandler<EntityDeletingEventData<Order>>, IScopedDependency
    {
        private readonly IExplainerDomainService _explainerDomainService;

        public OrderDeletingEventHandler(IExplainerDomainService explainerDomainService)
        {
            _explainerDomainService = explainerDomainService;
        }

        public async Task HandleEventAsync(EntityDeletingEventData<Order> eventData)
        {
            await CancelExplainer(eventData.Entity);
        }

        private async Task CancelExplainer(Order order)
        {
            await _explainerDomainService.CancelTimeslotAsync(order.OrderTypeId, order.Etime, order.ExplainerTimeId);
        }
    }
}
