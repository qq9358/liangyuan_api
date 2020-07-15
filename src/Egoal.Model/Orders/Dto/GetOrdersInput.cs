using Egoal.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Egoal.Orders.Dto
{
    public class GetOrdersInput : PagedInputDto
    {
        public DateTime? StartCTime { get; set; }
        public DateTime? EndCTime { get; set; }
        public string StartTravelDate { get; set; }
        public string EndTravelDate { get; set; }
        public Guid? CustomerId { get; set; }
        public bool? HasCustomer { get; set; }
        public string ListNo { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public ConsumeStatus? ConsumeStatus { get; set; }
        public RefundStatus? RefundStatus { get; set; }
        public OrderType? OrderType { get; set; }
        public string ContactName { get; set; }
        public string ContactMobile { get; set; }
        public bool NeedCheckTime { get; set; }
        public string Remark { get; set; }
    }
}
