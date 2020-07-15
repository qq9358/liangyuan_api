using Egoal.Tickets.Dto;
using System;
using System.Collections.Generic;

namespace Egoal.Orders.Dto
{
    public class OrderInfoDto
    {
        public OrderInfoDto()
        {
            Details = new List<OrderDetailDto>();
            Tourists = new List<TouristDto>();
        }

        public string ListNo { get; set; }
        public string OrderStatusName { get; set; }
        public string TravelDate { get; set; }
        public bool ShouldPay { get; set; }
        public bool? PayFlag { get; set; }
        public decimal TotalMoney { get; set; }
        public string PayTypeName { get; set; }
        public string Mobile { get; set; }
        public int PersonNum { get; set; }
        public string CTime { get; set; }
        public bool AllowCancel { get; set; }
        public string RefundStatusName { get; set; }
        public string ArrivalTime { get; set; }
        public string LicensePlateNumber { get; set; }
        public string JidiaoName { get; set; }
        public string KeYuanTypeName { get; set; }
        public string AreaName { get; set; }
        public string CashierName { get; set; }
        public string ExplainerName { get; set; }
        public string ExplainerTime { get; set; }
        public string CompanyName { get; set; }
        public string ChangCiName { get; set; }
        public long ExpireSeconds { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; }
        public bool AllowInvoice { get; set; }
        public bool AllowChangeChangCi { get; set; }
        public bool AllowChangeQuantity { get; set; }
        public object Contact { get; set; }
        public string Memo { get; set; }

        /// <summary>
        /// 票数量
        /// </summary>
        public int TicketsLength { get; set; }

        /// <summary>
        /// 导游编号
        /// </summary>
        public Guid? GuideId { get; set; }

        public List<OrderDetailDto> Details { get; set; }
        public List<TouristDto> Tourists { get; set; }
    }
}
