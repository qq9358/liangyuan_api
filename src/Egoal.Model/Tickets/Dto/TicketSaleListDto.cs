using Egoal.Application.Services.Dto;
using Egoal.Trades;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Tickets.Dto
{
    public class TicketSaleListDto : EntityDto<long>
    {
        [Display(Name = "序号")]
        public string RowNum { get; set; }

        [Display(Name = "单号")]
        public string ListNo { get; set; }

        [Display(Name = "售票时间")]
        public string CTime { get; set; }

        public int? TicketTypeId { get; set; }

        [Display(Name = "票类")]
        public string TicketTypeName { get; set; }

        [Display(Name = "票号")]
        public string TicketCode { get; set; }

        [Display(Name = "卡号")]
        public string CardNo { get; set; }

        public TicketStatus TicketStatusID { get; set; }

        [Display(Name = "状态")]
        public string TicketStatusName { get; set; }

        public FingerStatus? FingerStatusID { get; set; }

        public bool? ValidFlag { get; set; }

        [Display(Name = "是否有效")]
        public string ValidFlagName { get; set; }

        [Display(Name = "单价")]
        public decimal? RealPrice { get; set; }

        [Display(Name = "人数")]
        public int? PersonNum { get; set; }

        [Display(Name = "实售金额")]
        public decimal? RealMoney { get; set; }

        [Display(Name = "付款方式")]
        public string PayTypeName { get; set; }

        public TradeSource TradeSource { get; set; }

        [Display(Name = "购买类型")]
        public string TradeSourceName { get; set; }

        [Display(Name = "证件类型")]
        public string CertTypeName { get; set; }

        [Display(Name = "证件号码")]
        public string CertNo { get; set; }

        [Display(Name = "指纹登记数量")]
        public int? FingerprintNum { get; set; }

        [Display(Name = "指纹未登数量")]
        public int? UnBindFingerprintNum { get; set; }
        public bool? PhotoBindFlag { get; set; }

        [Display(Name = "人像登记状态")]
        public string PhotoBindFlagName { get; set; }

        [Display(Name = "人像登记时间")]
        public string PhotoBindTime { get; set; }

        public Guid? CustomerID { get; set; }

        [Display(Name = "客户")]
        public string CustomerName { get; set; }

        public Guid? MemberID { get; set; }

        [Display(Name = "会员")]
        public string MemberName { get; set; }

        public Guid? GuiderID { get; set; }

        [Display(Name = "导游")]
        public string GuiderName { get; set; }

        [JsonIgnore]
        public int? CashierId { get; set; }

        [Display(Name = "收银员")]
        public string CashierName { get; set; }

        [Display(Name = "收银机")]
        public string CashPCName { get; set; }

        [Display(Name = "售票点")]
        public string SalePointName { get; set; }

        [Display(Name = "售票景点")]
        public string ParkName { get; set; }

        [Display(Name = "总次数")]
        public int? TotalNum { get; set; }

        [Display(Name = "原始单价")]
        public decimal? TicPrice { get; set; }

        [Display(Name = "原始金额")]
        public decimal? TicMoney { get; set; }

        [Display(Name = "折扣类型")]
        public string DiscountTypeName { get; set; }

        [Display(Name = "折扣率")]
        public decimal? DiscountRate { get; set; }

        [Display(Name = "起始有效期")]
        public string STime { get; set; }

        [Display(Name = "最晚有效期")]
        public string ETime { get; set; }

        [Display(Name = "退票类型")]
        public string ReturnTypeName { get; set; }

        [Display(Name = "退票折扣率")]
        public decimal? ReturnRate { get; set; }

        [Display(Name = "订单单号")]
        public string OrderListNo { get; set; }

        [Display(Name = "第三方单号")]
        public string ThirdPartyPlatformOrderID { get; set; }

        [Display(Name = "备注")]
        public string Memo { get; set; }

        public string SalesmanName { get; set; }
    }
}
