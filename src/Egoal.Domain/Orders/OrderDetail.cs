using Egoal.Domain.Entities.Auditing;
using Egoal.Tickets;
using Egoal.TicketTypes;
using System;
using System.Collections.Generic;

namespace Egoal.Orders
{
    public class OrderDetail : AuditedEntity<long>
    {
        public string ListNo { get; set; }
        public OrderType? OrderTypeId { get; set; }
        public string TicketStime { get; set; }
        public string TicketEtime { get; set; }
        public int? TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public Guid? TicketTypeSyncCode { get; set; }
        public int? DiscountTypeId { get; set; }
        public string DiscountTypeName { get; set; }
        public Guid? DiscountTypeSyncCode { get; set; }
        public int TotalNum { get; set; }
        public int CollectNum { get; set; }
        public int UsedNum { get; set; }
        public int ReturnNum { get; set; }
        public int SurplusNum { get; set; }
        public int RefundPhysicalTicketNum { get; set; }
        public int? BeforeExchangeRefundQuantity { get; set; } = 0;
        public int? BeforeExchangeConsumeQuantity { get; set; } = 0;
        public decimal? TicPrice { get; set; }
        public decimal? ReaPrice { get; set; }
        public decimal? TicMoney { get; set; }
        public decimal ReaMoney { get; set; }
        public decimal? YaJinPrice { get; set; }
        public decimal? YaJin { get; set; }
        public DateTime? Stime { get; set; }
        public DateTime? Etime { get; set; }
        public string TouristName { get; set; }
        public string TicketCode { get; set; }
        public string CertNo { get; set; }
        public string Mobile { get; set; }
        public Guid? MemberId { get; set; }
        public string MemberName { get; set; }
        public Guid? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid? GuiderId { get; set; }
        public string GuiderName { get; set; }
        public bool? WeekendUse { get; set; }
        public int? ChangCiId { get; set; }
        public string Memo { get; set; }
        public string CheckRule { get; set; }
        public bool HasGroundSeat { get; set; }
        public bool HasGroundChangCi { get; set; }
        public bool? StatFlag { get; set; }
        public string Cdate { get; set; }
        public string Cweek { get; set; }
        public string Cmonth { get; set; }
        public string Cquarter { get; set; }
        public string Cyear { get; set; }
        public string Ctp { get; set; }
        public long? Bid { get; set; }
        public bool? CommitFlag { get; set; } = true;
        public bool? CertBlackListHandleFlag { get; set; }
        public int? ParkId { get; set; }
        public string ParkName { get; set; }
        public Guid SyncCode { get; set; } = Guid.NewGuid();

        public TicketType TicketType { get; set; }
        public TicketSaleBuyer Tourist { get; set; }

        public virtual Order Order { get; set; }
        public virtual ICollection<OrderGroundChangCi> OrderGroundChangCis { get; set; }
        public virtual ICollection<OrderTourist> OrderTourists { get; set; }

        public void SetTicketType(TicketType ticketType)
        {
            if (ticketType == null)
            {
                throw new TmsException($"票类不存在");
            }

            TicketType = ticketType;
            TicketTypeId = ticketType.Id;
            TicketTypeName = ticketType.Name;
            TicketTypeSyncCode = ticketType.SyncCode;
            StatFlag = ticketType.StatFlag;
        }

        public void SetTicMoney(int totalNum, decimal ticPrice)
        {
            TicPrice = ticPrice;
            TicMoney = ticPrice * totalNum;
        }

        public void SetReaMoney(int totalNum, decimal reaPrice)
        {
            ReaPrice = reaPrice;
            ReaMoney = totalNum * reaPrice;
        }

        public void Refund(int quantity)
        {
            ReturnNum += quantity;
            SurplusNum -= quantity;
            BeforeExchangeRefundQuantity += quantity;
        }
    }
}
