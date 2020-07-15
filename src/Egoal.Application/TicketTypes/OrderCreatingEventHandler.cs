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
    public class OrderCreatingEventHandler : IAsyncEventHandler<EntityCreatingEventData<Order>>, IScopedDependency
    {
        private readonly ITicketTypeDomainService _ticketTypeDomainService;

        public OrderCreatingEventHandler(ITicketTypeDomainService ticketTypeDomainService)
        {
            _ticketTypeDomainService = ticketTypeDomainService;
        }

        public async Task HandleEventAsync(EntityCreatingEventData<Order> eventData)
        {
            var order = eventData.Entity;

            foreach (var orderDetail in order.OrderDetails)
            {
                var saleStock = new TicketSaleStock();
                saleStock.TicketTypeId = orderDetail.TicketTypeId.Value;
                saleStock.TravelDate = order.Etime.To<DateTime>();
                saleStock.SaleNum = orderDetail.TotalNum;
                saleStock.CustomerId = order.CustomerId;
                await _ticketTypeDomainService.ReduceStockAsync(saleStock);
            }
        }
    }
}
