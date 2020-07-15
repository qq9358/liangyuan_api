using Egoal.Application.Services.Dto;
using Egoal.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Egoal.TicketTypes.Dto
{
    public class TicketTypeForSaleListDto : EntityDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        [JsonConverter(typeof(DateConverter))]
        public DateTime StartTravelDate { get; set; }
        public bool AllowRefund { get; set; }
        public bool RefundLimited { get; set; }
        public List<string> Classes { get; set; }
        public bool AllowInlandTourist { get; set; }
        public bool AllowOverseaTourist { get; set; }
        public bool AutoChooseByAge { get; set; }
        public bool FirstActiveFlag { get; set; }
        public StatGroup? StatGroupId { get; set; }
        public bool WxShowQrCode { get; set; }
        public string UsageMethod { get; set; }

        /// <summary>
        /// 票类年龄范围
        /// </summary>
        public List<TicketTypeAgeRange> TicketTypeAgeRanges { get; set; }
    }

    /// <summary>
    /// 票类年龄范围
    /// </summary>
    public class TicketTypeAgeRange
    {
        /// <summary>
        /// 票类型标识
        /// </summary>
        public int TicketTypeId { get; set; }

        /// <summary>
        /// 起始年龄
        /// </summary>
        public int StartAge { get; set; }

        /// <summary>
        /// 截止年龄
        /// </summary>
        public int EndAge { get; set; }
    }
}
