using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Tickets.Dto
{
    public class StatTicketConsumeListDto
    {
        [JsonIgnore]
        public Guid? CustomerId { get; set; }
        [Display(Name = "客户")]
        public string CustomerName { get; set; }

        [JsonIgnore]
        public int TicketTypeId { get; set; }
        [Display(Name = "票类")]
        public string TicketTypeName { get; set; }

        [Display(Name = "单价")]
        public decimal? Price { get; set; }

        [Display(Name = "检票数量")]
        public int? CheckNum { get; set; }

        [Display(Name = "检票金额")]
        public decimal? CheckMoney { get; set; }

        [Display(Name = "核销数量")]
        public int? ConsumeNum { get; set; }

        [Display(Name = "核销金额")]
        public decimal? ConsumeMoney { get; set; }
    }
}
