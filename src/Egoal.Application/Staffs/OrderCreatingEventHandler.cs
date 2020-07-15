using Egoal.Dependency;
using Egoal.Events.Bus.Entities;
using Egoal.Events.Bus.Handlers;
using Egoal.Orders;
using System.Threading.Tasks;

namespace Egoal.Staffs
{
    public class OrderCreatingEventHandler : IAsyncEventHandler<EntityCreatingEventData<Order>>, IScopedDependency
    {
        private readonly IExplainerDomainService _explainerDomainService;

        public OrderCreatingEventHandler(IExplainerDomainService explainerDomainService)
        {
            _explainerDomainService = explainerDomainService;
        }

        public async Task HandleEventAsync(EntityCreatingEventData<Order> eventData)
        {
            await BookExplainer(eventData.Entity);
        }

        private async Task BookExplainer(Order order)
        {
            await _explainerDomainService.BookTimeslotAsync(order.OrderTypeId, order.Etime, order.ExplainerTimeId);
        }
    }
}
