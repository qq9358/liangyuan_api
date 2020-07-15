using Egoal.Application.Services.Dto;
using System;

namespace Egoal.Orders.Dto
{
    public class GetMemberOrdersForQueryInput : PagedInputDto
    {
        /// <summary>
        /// 下单日期开始
        /// </summary>
        public DateTime? CTimeFrom { get; set; }

        /// <summary>
        /// 下单日期结束
        /// </summary>
        public DateTime? CTimeTo { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string ListNo { get; set; }

        /// <summary>
        /// 游玩日期开始
        /// </summary>
        public DateTime? EtimeFrom { get; set; }

        /// <summary>
        /// 游玩日期结束
        /// </summary>
        public DateTime? EtimeTo { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string OrderStatusName { get; set; }
    }
}
