using Egoal.Domain.Repositories;
using Egoal.Tickets.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public interface ITicketSaleBuyerRepository : IRepository<TicketSaleBuyer, long>
    {
        Task<int> GetCertNoBuyQuantity(string certNo, DateTime startTime, DateTime endTime);
        Task<DataTable> StatTouristByAgeRangeAsync(StatTouristByAgeRangeInput input);
        Task<DataTable> StatTouristByAreaAsync(StatTouristByAreaInput input);
        Task<DataTable> StatTouristBySexAsync(StatTouristBySexInput input);
        Task<List<StatTouristListDto>> StatTouristAsync(StatTouristInput input);
    }
}
