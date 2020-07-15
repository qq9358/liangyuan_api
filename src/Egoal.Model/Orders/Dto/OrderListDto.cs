using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Orders.Dto
{
    public class OrderListDto
    {
        [Display(Name = "单号")]
        public string ListNo { get; set; }

        [Display(Name = "下单时间")]
        public string CTime { get; set; }

        [Display(Name = "总金额")]
        public decimal TotalMoney { get; set; }

        [Display(Name = "总数量")]
        public int TotalNum { get; set; }

        [Display(Name = "已出票")]
        public int CollectNum { get; set; }

        [Display(Name = "已退")]
        public int ReturnNum { get; set; }

        [Display(Name = "已用")]
        public int UsedNum { get; set; }

        [Display(Name = "未用")]
        public int SurplusNum { get; set; }

        [Display(Name = "游玩日期")]
        public string TravelDate { get; set; }

        [Display(Name = "联系人")]
        public string YdrName { get; set; }

        [Display(Name = "手机号码")]
        public string Mobile { get; set; }

        [JsonIgnore]
        public OrderStatus OrderStatusId { get; set; }

        [Display(Name = "订单状态")]
        public string OrderStatusName { get; set; }

        [JsonIgnore]
        public ConsumeStatus? ConsumeStatus { get; set; }

        [Display(Name = "消费状态")]
        public string ConsumeStatusName { get; set; }

        [JsonIgnore]
        public RefundStatus? RefundStatus { get; set; }

        [Display(Name = "退款状态")]
        public string RefundStatusName { get; set; }

        [JsonIgnore]
        public OrderType OrderTypeId { get; set; }

        [Display(Name = "订单类型")]
        public string OrderTypeName { get; set; }

        [Display(Name = "客户")]
        public string CustomerName { get; set; }

        [Display(Name = "会员")]
        public string MemberName { get; set; }

        [Display(Name = "导游")]
        public string GuiderName { get; set; }

        [Display(Name = "是否付款")]
        public bool? PayFlag { get; set; }

        [Display(Name = "第三方单号")]
        public string ThirdListNo { get; set; }

        [Display(Name = "备注")]
        public string Memo { get; set; }

        [JsonIgnore]
        public int? ExplainerId { get; set; }
        public string ExplainerName { get; set; }
        [JsonIgnore]
        public int? ExplainerTimeId { get; set; }
        public string ExplainerTimeslotName { get; set; }
        public string JidiaoName { get; set; }
        public string JidiaoMobile { get; set; }
        public bool AllowCancel { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }
}
