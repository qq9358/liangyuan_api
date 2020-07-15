using Egoal.Application.Services.Dto;
using Egoal.Tickets.Dto;
using System.Collections.Generic;

namespace Egoal.Orders.Dto
{
    public class OrderDetailDto : EntityDto<long>
    {
        public int? TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public int TotalNum { get; set; }
        public int SurplusNum { get; set; }
        public int UsableQuantity { get; set; }
        public decimal ReaPrice { get; set; }
        public string ETime { get; set; }

        /// <summary>
        /// 第一次使用需要激活
        /// </summary>
        public bool FirstActiveFlag { get; set; }

        /// <summary>
        /// 微信是否显示二维码
        /// </summary>
        public bool WxShowQrCode { get; set; }

        /// <summary>
        /// 微信购票说明文字
        /// </summary>
        public string UsageMethod { get; set; }

        public List<TicketSaleSimpleDto> Tickets { get; set; }
        public List<OrderGroundChangCiDto> GroundChangCis { get; set; }
    }
}
