using Egoal.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public interface ITicketSaleStockRepository : IRepository<TicketSaleStock>
    {
        Task<int> GetSaleQuantityAsync(int ticketTypeId, DateTime startDate, DateTime endDate);
        Task<bool> UpdateStockAsync(TicketSaleStock saleStock);
    }
}
