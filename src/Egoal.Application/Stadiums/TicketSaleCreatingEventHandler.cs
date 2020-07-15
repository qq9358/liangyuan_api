using Egoal.Dependency;
using Egoal.Events.Bus.Entities;
using Egoal.Events.Bus.Handlers;
using Egoal.Extensions;
using Egoal.Tickets;
using System.Threading.Tasks;

namespace Egoal.Stadiums
{
    public class TicketSaleCreatingEventHandler : IAsyncEventHandler<EntityCreatingEventData<TicketSale>>, IScopedDependency
    {
        private readonly ISeatStatusCacheRepository _seatStatusCacheRepository;

        public TicketSaleCreatingEventHandler(ISeatStatusCacheRepository seatStatusCacheRepository)
        {
            _seatStatusCacheRepository = seatStatusCacheRepository;
        }

        public async Task HandleEventAsync(EntityCreatingEventData<TicketSale> eventData)
        {
            var ticketSale = eventData.Entity;

            if (ticketSale.TicketSaleSeats.IsNullOrEmpty())
            {
                return;
            }

            foreach (var seat in ticketSale.TicketSaleSeats)
            {
                var seatStatusCache = await _seatStatusCacheRepository.FirstOrDefaultAsync(s => s.ListNo == ticketSale.OrderListNo && s.SeatId == seat.SeatId);
                seatStatusCache.TradeId = ticketSale.TradeId;
                seatStatusCache.TicketId = ticketSale.Id;
            }
        }
    }
}
