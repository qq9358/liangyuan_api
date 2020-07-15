using Egoal.Stadiums.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Egoal.Stadiums
{
    public interface IStadiumDomainService
    {
        Task<int> GetSeatSurplusQuantityAsync(DateTime date, int changCiId, int stadiumId);
        Task<List<long>> SeatingAsync(SeatingInput input);
        Task CancelSeatingAsync(string listNo, long? ticketId, int quantity);
    }
}
