using Egoal.Dependency;
using Egoal.Events.Bus.Entities;
using Egoal.Events.Bus.Handlers;
using Egoal.Extensions;
using Egoal.Orders;
using Egoal.Tickets;
using System;
using System.Threading.Tasks;

namespace Egoal.TicketTypes
{
    public class OrderDeletingEventHandler : IAsyncEventHandler<EntityDeletingEventData<Order>>, IScopedDependency
    {
        private readonly ITicketTypeDomainService _ticketTypeDomainService;

        public OrderDeletingEventHandler(ITicketTypeDomainService ticketTypeDomainService)
        {
            _ticketTypeDomainService = ticketTypeDomainService;
        }

        public async Task HandleEventAsync(EntityDeletingEventData<Order> eventData)
        {
            var order = eventData.Entity;

            foreach (var orderDetail in order.OrderDetails)
            {
                var saleStock = new TicketSaleStock();
                saleStock.TicketTypeId = orderDetail.TicketTypeId.Value;
                saleStock.TravelDate = order.Etime.To<DateTime>();
                saleStock.SaleNum = orderDetail.TotalNum;
                saleStock.CustomerId = order.CustomerId;
                await _ticketTypeDomainService.IncreaseStockAsync(saleStock);
            }
        }
    }
}
