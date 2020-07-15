using Egoal.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Orders.Dto
{
    public class GetOrderInfoInput
    {
        [Display(Name = "单号")]
        [MustFillIn]
        public string ListNo { get; set; }
        public DateTime? StartCTime { get; set; }
        public DateTime? EndCTime { get; set; }
        public int? CashierId { get; set; }
        public int? CashPcid { get; set; }
        public int? SalePointId { get; set; }
        public int? ParkId { get; set; }
    }
}
