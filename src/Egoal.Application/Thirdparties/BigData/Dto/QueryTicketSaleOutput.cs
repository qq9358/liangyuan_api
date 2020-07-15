using System.Collections.Generic;

namespace Egoal.Thirdparties.BigData.Dto
{
    public class QueryTicketSaleOutput
    {
        public QueryTicketSaleOutput()
        {
            tickets = new List<TicketSaleListDto>();
        }

        public int total_quantity { get; set; }
        public List<TicketSaleListDto> tickets { get; set; }
    }
}
