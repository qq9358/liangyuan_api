using System.Collections.Generic;

namespace Egoal.Orders.Dto
{
    public class OrderOptionsTicketDto:OrderOptionsDto
    {
        public OrderOptionsTicketDto()
        {
            DateTickets = new List<dynamic>();
        }

        /// <summary>
        /// 购票日期附带票状态
        /// </summary>
        public List<dynamic> DateTickets { get; set; }
    }
}
