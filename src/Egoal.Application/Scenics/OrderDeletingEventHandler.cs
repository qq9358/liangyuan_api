using Egoal.Dependency;
using Egoal.Domain.Repositories;
using Egoal.Events.Bus.Entities;
using Egoal.Events.Bus.Handlers;
using Egoal.Orders;
using Egoal.Scenics.Dto;
using System.Threading.Tasks;

namespace Egoal.Scenics
{
    public class OrderDeletingEventHandler : IAsyncEventHandler<EntityDeletingEventData<Order>>, IScopedDependency
    {
        private readonly IScenicAppService _scenicAppService;
        private readonly IRepository<OrderGroundChangCi, long> _orderGroundChangCiRepository;

        public OrderDeletingEventHandler(
            IScenicAppService scenicAppService,
            IRepository<OrderGroundChangCi, long> orderGroundChangCiRepository)
        {
            _scenicAppService = scenicAppService;
            _orderGroundChangCiRepository = orderGroundChangCiRepository;
        }

        public async Task HandleEventAsync(EntityDeletingEventData<Order> eventData)
        {
            var order = eventData.Entity;

            foreach (var orderDetail in order.OrderDetails)
            {
                if (!orderDetail.HasGroundSeat && !orderDetail.HasGroundChangCi)
                {
                    continue;
                }

                var groundChangCis = await _orderGroundChangCiRepository.GetAllListAsync(o => o.OrderDetailId == orderDetail.Id);
                foreach (var groundChangCi in groundChangCis)
                {
                    CancelGroundChangCiInput cancelGroundChangCiInput = new CancelGroundChangCiInput();
                    cancelGroundChangCiInput.HasGroundSeat = orderDetail.HasGroundSeat;
                    cancelGroundChangCiInput.HasGroundChangCi = orderDetail.HasGroundChangCi;
                    cancelGroundChangCiInput.ListNo = order.Id;
                    cancelGroundChangCiInput.GroundId = groundChangCi.GroundId;
                    cancelGroundChangCiInput.Date = order.Etime;
                    cancelGroundChangCiInput.ChangCiId = groundChangCi.ChangCiId;
                    cancelGroundChangCiInput.Quantity = groundChangCi.Quantity;

                    await _scenicAppService.CancelGroundChangCiAsync(cancelGroundChangCiInput);
                }
            }
        }
    }
}
