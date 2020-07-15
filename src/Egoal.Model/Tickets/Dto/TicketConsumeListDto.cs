using Egoal.Application.Services.Dto;
using Egoal.Extensions;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Tickets.Dto
{
    public class TicketConsumeListDto : EntityDto<long>
    {
        [Display(Name = "序号")]
        public string RowNum { get; set; }

        [Display(Name = "检票时间")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? ConsumeTime { get; set; }

        [JsonIgnore]
        public int TicketTypeId { get; set; }
        [Display(Name = "票类")]
        public string TicketTypeName { get; set; }

        [Display(Name = "票号")]
        public string TicketCode { get; set; }

        [Display(Name = "单价")]
        public decimal? Price { get; set; }

        [Display(Name = "核销数量")]
        public int? ConsumeNum { get; set; }

        [Display(Name = "核销金额")]
        public decimal? ConsumeMoney { get; set; }

        [Display(Name = "总数量")]
        public int? TotalNum { get; set; }

        [JsonIgnore]
        public ConsumeType ConsumeType { get; set; }
        [Display(Name = "核销类型")]
        public string ConsumeTypeName { get; set; }

        [Display(Name = "客户")]
        public string CustomerName { get; set; }

        [Display(Name = "单号")]
        public string ListNo { get; set; }

        [Display(Name = "第三方单号")]
        public string ThirdOrderId { get; set; }

        [Display(Name = "核销时间")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? LastNoticeTime { get; set; }
    }
}
