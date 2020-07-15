using Egoal.Domain.Entities.Auditing;
using Egoal.Extensions;
using Egoal.Payment;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Egoal.Orders
{
    public class Order : AuditedEntity<string>
    {
        public Order() : this(DateTime.Now)
        {
        }

        public Order(DateTime ctime)
        {
            OrderDetails = new List<OrderDetail>();
            OrderAgeRanges = new List<OrderAgeRange>();

            CTime = ctime;
            Cdate = ctime.ToDateString();
            Cweek = ctime.ToWeekString();
            Cmonth = ctime.ToMonthString();
            Cquarter = ctime.ToQuarterString();
            Cyear = ctime.ToYearString();
            Ctp = ctime.ToHourString();
        }

        public OrderType OrderTypeId { get; set; }
        public string OrderTypeName { get; set; }
        public OrderStatus OrderStatusId { get; set; }
        public string OrderStatusName { get; set; }
        public ConsumeStatus? ConsumeStatus { get; set; } = Orders.ConsumeStatus.未消费;
        public RefundStatus? RefundStatus { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; } = InvoiceStatus.未开票;
        public int? QuPiaoTypeId { get; set; }
        public string QuPiaoTypeName { get; set; }
        public int? ExpressTypeId { get; set; }
        public string ExpressTypeName { get; set; }
        public decimal? ExpressFee { get; set; }
        public string Etime { get; set; }
        public string ArrivalDate { get; set; }
        public string ArrivalTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? PayFlag { get; set; } = false;
        public int? PayTypeId { get; set; }
        public string PayTypeName { get; set; }
        public decimal TotalMoney { get; set; }
        public decimal? TicketMoney { get; set; }
        public decimal? DingJin { get; set; } = 0;
        public int TotalNum { get; set; }
        public int CollectNum { get; set; }
        public int UsedNum { get; set; }
        public int ReturnNum { get; set; }
        public int SurplusNum { get; set; }
        public int? KeYuanTypeId { get; set; }
        public int? KeYuanAreaId { get; set; }
        public int? SalesmanId { get; set; }
        public Guid? MemberId { get; set; }
        public string MemberName { get; set; }
        public Guid? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid? GuiderId { get; set; }
        public string GuiderName { get; set; }
        public decimal? GuidFee { get; set; }
        public int? ExplainerId { get; set; }
        public int? ExplainerTimeId { get; set; }
        public int? ChangCiId { get; set; }
        public string JidiaoName { get; set; }
        public string JidiaoMobile { get; set; }
        public int? CertTypeId { get; set; }
        public string CertTypeName { get; set; }
        public string CertNo { get; set; }
        public string YdrName { get; set; }
        public string Mobile { get; set; }
        public string Pwd { get; set; }
        public string Memo { get; set; }
        public int? CarType { get; set; }
        public string LicensePlateNumber { get; set; }
        public bool? OrderSmsSendFlag { get; set; }
        public DateTime? OrderSmsSendTime { get; set; }
        public bool? PaySmsSendFlag { get; set; }
        public DateTime? PaySmsSendTime { get; set; }
        public string ThirdPartyPlatformOrderId { get; set; }
        public string ThirdPartyPlatformId { get; set; }
        public int? Vid { get; set; }
        public string Vname { get; set; }
        public DateTime? Vtime { get; set; }
        public DateTime? PayTime { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public bool? StatFlag { get; set; }
        public string Cdate { get; set; }
        public string Cweek { get; set; }
        public string Cmonth { get; set; }
        public string Cquarter { get; set; }
        public string Cyear { get; set; }
        public string Ctp { get; set; }
        public long? Bid { get; set; }
        public bool? PrintFlag { get; set; }
        public bool? PrintInvoiceFlag { get; set; }
        public bool? CommitFlag { get; set; } = true;
        public int? CashierId { get; set; }
        public int? CashPcId { get; set; }
        public int? SalePointId { get; set; }
        public int? ParkId { get; set; }
        public string ParkName { get; set; }
        public Guid SyncCode { get; set; } = Guid.NewGuid();

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<OrderAgeRange> OrderAgeRanges { get; set; }

        public void Sum()
        {
            TotalNum = OrderDetails.Sum(o => o.TotalNum);
            SurplusNum = OrderDetails.Sum(o => o.SurplusNum);
            TotalMoney = OrderDetails.Sum(o => o.ReaMoney);
            TicketMoney = OrderDetails.Sum(o => o.TicMoney);
        }

        public void Pay(int payTypeId, string payTypeName)
        {
            PayFlag = true;
            PayTypeId = payTypeId;
            PayTypeName = payTypeName;
            PayTime = DateTime.Now;
        }

        public void Audit()
        {
            OrderStatusId = OrderStatus.Audited;
            OrderStatusName = "已审核";
        }

        public bool ShouldPay()
        {
            return !IsFree() && PayFlag != true && OrderStatusId == OrderStatus.Audited;
        }

        public bool HasPaid()
        {
            return !IsFree() && PayFlag == true;
        }

        public bool IsFree()
        {
            return TotalMoney == 0;
        }

        public void Cancel()
        {
            OrderStatusId = OrderStatus.Canceled;
            OrderStatusName = "已取消";
            if (HasPaid())
            {
                RefundStatus = Orders.RefundStatus.已退款;
            }
            ReturnNum = ReturnNum + SurplusNum;
            SurplusNum = 0;

            foreach (var orderDetail in OrderDetails)
            {
                orderDetail.ReturnNum = orderDetail.ReturnNum + orderDetail.SurplusNum;
                orderDetail.BeforeExchangeRefundQuantity = orderDetail.BeforeExchangeRefundQuantity + orderDetail.SurplusNum;
                orderDetail.SurplusNum = 0;
            }
        }

        public void Consume(OrderDetail orderDetail, int consumeNum)
        {
            UsedNum += consumeNum;
            SurplusNum -= consumeNum;
            if (UsedNum == TotalNum)
            {
                ConsumeStatus = Orders.ConsumeStatus.已消费;
            }
            else if (UsedNum > 0)
            {
                ConsumeStatus = Orders.ConsumeStatus.部分消费;
            }

            if (SurplusNum <= 0)
            {
                OrderStatusId = OrderStatus.Completed;
                OrderStatusName = "已完成";
            }

            orderDetail.UsedNum += consumeNum;
            orderDetail.SurplusNum -= consumeNum;
        }

        public void ChangeExplainer(int newExplainerId)
        {
            ExplainerId = newExplainerId;
        }

        public void Refund(int quantity)
        {
            ReturnNum += quantity;
            SurplusNum -= quantity;

            if (ReturnNum == TotalNum)
            {
                OrderStatusId = OrderStatus.Canceled;
                OrderStatusName = "已取消";
            }
            else if (SurplusNum <= 0)
            {
                OrderStatusId = OrderStatus.Completed;
                OrderStatusName = "已完成";
            }

            SetRefundStatus();
        }

        public void SetRefundStatus()
        {
            if (IsFree() || ReturnNum <= 0)
            {
                RefundStatus = null;
            }
            else if (ReturnNum == TotalNum)
            {
                RefundStatus = Orders.RefundStatus.已退款;
            }
            else
            {
                RefundStatus = Orders.RefundStatus.部分退款;
            }
        }
    }
}
