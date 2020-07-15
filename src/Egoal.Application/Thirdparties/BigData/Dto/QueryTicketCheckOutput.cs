using System.Collections.Generic;

namespace Egoal.Thirdparties.BigData.Dto
{
    public class QueryTicketCheckOutput
    {
        public QueryTicketCheckOutput()
        {
            tickets = new List<TicketCheckListDto>();
        }

        public int total_quantity { get; set; }
        public List<TicketCheckListDto> tickets { get; set; }
    }
}
