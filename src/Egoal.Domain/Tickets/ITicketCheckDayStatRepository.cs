using Egoal.Domain.Repositories;
using Egoal.Tickets.Dto;
using System.Data;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    public interface ITicketCheckDayStatRepository : IRepository<TicketCheckDayStat, long>
    {
        Task<DataTable> GetStadiumTicketCheckOverviewAsync(string startDate, string endDate);
        Task<int> GetScenicCheckInQuantityAsync(string startDate, string endDate);
        Task<int> GetScenicCheckOutQuantityAsync(string startDate, string endDate);
        Task<DataTable> StatStadiumTicketCheckInAsync(StatTicketCheckInInput input);
        Task<DataTable> StatTicketCheckInYearOverYearComparisonAsync(StatTicketCheckInInput input);
        Task<DataTable> StatTicketCheckInAverageByTimeslotAsync(StatTicketCheckInInput input);
        Task<DataTable> StatTicketCheckInAverageByDateAsync(StatTicketCheckInInput input);
        Task<DataTable> StatTicketCheckInAverageByMonthAsync(StatTicketCheckInInput input);
    }
}
