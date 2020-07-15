using Egoal.Dependency;
using Egoal.Events.Bus.Handlers;
using Egoal.Extensions;
using Egoal.Tickets;
using System;
using System.Threading.Tasks;

namespace Egoal.TicketTypes
{
    public class RefundTicketEventHandler : IAsyncEventHandler<RefundTicketEventData>, IScopedDependency
    {
        private readonly ITicketTypeDomainService _ticketTypeDomainService;

        public RefundTicketEventHandler(ITicketTypeDomainService ticketTypeDomainService)
        {
            _ticketTypeDomainService = ticketTypeDomainService;
        }

        public async Task HandleEventAsync(RefundTicketEventData eventData)
        {
            foreach (var item in eventData.Items)
            {
                var ticketSale = item.OriginalTicketSale;

                var saleStock = new TicketSaleStock();
                saleStock.TicketTypeId = ticketSale.TicketTypeId.Value;
                saleStock.TravelDate = ticketSale.Sdate.To<DateTime>();
                saleStock.SaleNum = item.RefundQuantity;
                saleStock.CustomerId = ticketSale.CustomerId;
                await _ticketTypeDomainService.IncreaseStockAsync(saleStock);
            }
        }
    }
}
