using Egoal.Domain.Repositories;
using Egoal.Tickets.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Stadiums
{
    public interface ISeatStatusCacheRepository : IRepository<SeatStatusCache, decimal>
    {
        Task<int> GetDisabledSeatQuantityAsync(DateTime date, int changCiId, int stadiumId, int lockTime);
        Task<bool> ReSaleAsync(decimal id, string listNo, SeatStatus seatStatus, DateTime lockTime);
        Task SaleAsync(SeatStatusCache seatStatusCache);
        Task CancelAsync(string listNo);
        Task<List<TicketSaleSeatDto>> GetOrderSeatsAsync(string listNo);
    }
}
