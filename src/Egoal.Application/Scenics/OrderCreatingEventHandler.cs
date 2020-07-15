using Egoal.Dependency;
using Egoal.Events.Bus.Entities;
using Egoal.Events.Bus.Handlers;
using Egoal.Extensions;
using Egoal.Orders;
using System.Threading.Tasks;

namespace Egoal.Scenics
{
    public class OrderCreatingEventHandler : IAsyncEventHandler<EntityCreatingEventData<Order>>, IScopedDependency
    {
        private readonly IScenicAppService _scenicAppService;

        public OrderCreatingEventHandler(IScenicAppService scenicAppService)
        {
            _scenicAppService = scenicAppService;
        }

        public async Task HandleEventAsync(EntityCreatingEventData<Order> eventData)
        {
            var order = eventData.Entity;
            foreach (var orderDetail in order.OrderDetails)
            {
                if (orderDetail.OrderGroundChangCis.IsNullOrEmpty())
                {
                    continue;
                }

                foreach (var groundChangCi in orderDetail.OrderGroundChangCis)
                {
                    var output = await _scenicAppService.BookGroundChangCiAsync(groundChangCi.GroundId, order.Etime, groundChangCi.ChangCiId, groundChangCi.Quantity, order.Id);
                    orderDetail.HasGroundSeat = output.HasGroundSeat;
                    orderDetail.HasGroundChangCi = output.HasGroundChangCi;
                }
            }
        }
    }
}
