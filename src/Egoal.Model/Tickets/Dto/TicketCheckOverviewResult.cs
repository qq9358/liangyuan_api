using System.Data;

namespace Egoal.Tickets.Dto
{
    public class TicketCheckOverviewResult
    {
        public int ScenicCheckInQuantity { get; set; }
        public int ScenicCheckOutQuantity { get; set; }
        public int ScenicRealTimeQuantity { get; set; }
        public DataTable StadiumOverview { get; set; }
    }
}
