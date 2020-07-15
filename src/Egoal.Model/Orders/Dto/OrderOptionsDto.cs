using System.Collections.Generic;

namespace Egoal.Orders.Dto
{
    public class OrderOptionsDto
    {
        public OrderOptionsDto()
        {
            Dates = new List<dynamic>();
            DisabledWeeks = new List<int>();
        }

        public List<dynamic> Dates { get; set; }
        public List<int> DisabledWeeks { get; set; }
        public int GroupMaxBuyQuantityPerOrder { get; set; }
        public int WeChatMaxBuyQuantityPerDay { get; set; }
    }
}
