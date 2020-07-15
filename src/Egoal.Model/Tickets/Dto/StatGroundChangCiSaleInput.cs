using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Tickets.Dto
{
    public class StatGroundChangCiSaleInput
    {
        [Display(Name = "销售日期")]
        [MustFillIn]
        public string TravelDate { get; set; }
        public int Week { get; set; }
    }
}
