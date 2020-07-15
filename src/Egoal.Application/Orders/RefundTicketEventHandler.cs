using Egoal.BackgroundJobs;
using Egoal.Dependency;
using Egoal.Events.Bus.Handlers;
using Egoal.Extensions;
using Egoal.Tickets;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Orders
{
    public class RefundTicketEventHandler : IAsyncEventHandler<RefundTicketEventData>, IScopedDependency
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDomainService _orderDomainService;
        private readonly IBackgroundJobService _backgroundJobService;

        public RefundTicketEventHandler(
            IOrderRepository orderRepository,
            IOrderDomainService orderDomainService,
            IBackgroundJobService backgroundJobService)
        {
            _orderRepository = orderRepository;
            _orderDomainService = orderDomainService;
            _backgroundJobService = backgroundJobService;
        }

        public async Task HandleEventAsync(RefundTicketEventData eventData)
        {
            var hasOrder = eventData.Items.Any(i => i.OriginalTicketSale.OrderDetailId.HasValue);
            if (!hasOrder) return;

            var order = await _orderRepository.GetAllIncluding(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == eventData.PayListNo);
            if (order == null) return;

            foreach (var item in eventData.Items)
            {
                if (!item.OriginalTicketSale.OrderDetailId.HasValue) continue;

                var orderDetail = order.OrderDetails.FirstOrDefault(o => o.Id == item.OriginalTicketSale.OrderDetailId);
                if (orderDetail == null) continue;

                orderDetail.Refund(item.RefundQuantity);
            }

            var totalRefundQuantity = eventData.Items.Sum(i => i.RefundQuantity);
            order.Refund(totalRefundQuantity);

            await _orderDomainService.BookChangCiAsync(order.Etime, order.ChangCiId.Value, totalRefundQuantity, true);

            var orderStat = new OrderStat();
            orderStat.Cdate = order.Etime;
            orderStat.OrderNum = -totalRefundQuantity;
            orderStat.OrderPlanType = 0;
            await _backgroundJobService.EnqueueAsync<UpdateOrderStatJob>(orderStat.ToJson());
        }
    }
}
