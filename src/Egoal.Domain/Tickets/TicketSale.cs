using Egoal.Domain.Entities;
using Egoal.Extensions;
using Egoal.Scenics.Dto;
using Egoal.TicketTypes;
using Egoal.Trades;
using Egoal.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Egoal.Tickets
{
    public class TicketSale : Entity<long>
    {
        public TicketSale()
        {
            TicketGrounds = new List<TicketGround>();
            TicketGroundCaches = new List<TicketGroundCache>();
        }

        public Guid TradeId { get; set; }
        public string ListNo { get; set; }
        public string TicketCode { get; set; }
        public string CardNo { get; set; }
        public string TdCode { get; set; }
        public string InvoiceCode { get; set; }
        public string InvoiceNo { get; set; }
        public bool? BindFlag { get; set; }
        public FingerStatus? FingerStatusId { get; set; }
        public bool? PhotoBindFlag { get; set; }
        public string YsqTicketCode { get; set; }
        public bool? ValidFlag { get; set; } = true;
        public string ValidFlagName { get; set; } = "有效";
        public TicketStatus TicketStatusId { get; set; }
        public string TicketStatusName { get; set; }
        public TicketTypeType? TicketTypeTypeId { get; set; }
        public int? TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public int? TicketTypeProjectId { get; set; }
        public int? TicketTypeProjectTypeId { get; set; }
        public int? TicketBindTypeId { get; set; }
        public TicketKind? Tkid { get; set; }
        public string Tkname { get; set; }
        public int? Ttid { get; set; }
        public int? PrintNum { get; set; } = 0;
        public int? PriceTypeId { get; set; }
        public string PriceTypeName { get; set; }
        public int? DiscountTypeId { get; set; }
        public string DiscountTypeName { get; set; }
        public decimal? DiscountRate { get; set; } = 100M;
        public int? DiscountApproverId { get; set; }
        public string DiscountApproverName { get; set; }
        public decimal? TicPrice { get; set; }
        public decimal? ReaPrice { get; set; }
        public decimal? PrintPrice { get; set; }
        public decimal? TicMoney { get; set; }
        public decimal? ReaMoney { get; set; }
        public decimal? PrintMoney { get; set; }
        public decimal? YaJin { get; set; } = 0;
        public decimal? YaJinWb { get; set; } = 0;
        public decimal? CardMoney { get; set; } = 0;
        public decimal? FreeMoney { get; set; } = 0;
        public decimal? TotalMoney { get; set; } = 0;
        public decimal? GuiderSharingMoney { get; set; } = 0;
        public decimal? Overdraw { get; set; } = 0;
        public int? PayTypeId { get; set; }
        public string PayTypeName { get; set; }
        public bool? PayFlag { get; set; } = false;
        public bool? CzkFlag { get; set; }
        public long? CzkId { get; set; }
        public string CzkTicketCode { get; set; }
        public string CzkCardNo { get; set; }
        public string CzkOwner { get; set; }
        public string CzkOwnerTel { get; set; }
        public int? CurrencyId { get; set; } = 1;
        public string CurrencyName { get; set; } = "人民币";
        public decimal? CurrencyRate { get; set; } = 1M;
        public decimal? WbReaPrice { get; set; }
        public decimal? WbReaMoney { get; set; }
        public bool? DrpFlag { get; set; }
        public int? TicketNum { get; set; } = 1;
        public int? FreeNum { get; set; } = 0;
        public int? PersonNum { get; set; }
        public int? TotalNum { get; set; }
        public int? SurplusNum { get; set; }
        public int? RestAuthorizationNum { get; set; } = 0;
        public string Stime { get; set; }
        public string Etime { get; set; }
        public string Sdate { get; set; }
        public int? CashierId { get; set; }
        public string CashierName { get; set; }
        public int? CashPcid { get; set; }
        public string CashPcname { get; set; }
        public int? SalePointId { get; set; }
        public string SalePointName { get; set; }
        public int? SalesmanId { get; set; }
        public string SalesmanName { get; set; }
        public Guid? MemberId { get; set; }
        public string MemberName { get; set; }
        public Guid? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid? GuiderId { get; set; }
        public string GuiderName { get; set; }
        public Guid? ManagerId { get; set; }
        public string ManagerName { get; set; }
        public int? PermitStaffId { get; set; }
        public int? AreaId { get; set; }
        public string AreaName { get; set; }
        public int? KeYuanTypeId { get; set; }
        public string KeYuanTypeName { get; set; }
        public long? ReturnTicketId { get; set; }
        public DateTime? SettleTime { get; set; }
        public long? AuthorizedTicketId { get; set; }
        public int? ReturnTypeId { get; set; }
        public string ReturnTypeName { get; set; }
        public decimal? ReturnRate { get; set; }
        public int? ReturnApproverId { get; set; }
        public string ReturnApproverName { get; set; }
        public string OrderListNo { get; set; }
        public long? OrderDetailId { get; set; }
        public int? CertTypeId { get; set; }
        public string CertTypeName { get; set; }
        public string CertNo { get; set; }
        public string Memo { get; set; }
        public string CheckRule { get; set; }
        public int? ChangCiId { get; set; }
        public int? GroundId { get; set; }
        public bool? StatFlag { get; set; }
        public bool? BdFlag { get; set; } = false;
        public string Cdate { get; set; }
        public string Cweek { get; set; }
        public string Cmonth { get; set; }
        public string Cquarter { get; set; }
        public string Cyear { get; set; }
        public string Ctp { get; set; }
        public DateTime? Ctime { get; set; }
        public DateTime? Ltime { get; set; } = DateTime.Now;
        public long? Bid { get; set; }
        public CheckType? CheckTypeId { get; set; }
        public bool? IsRechargeFlag { get; set; }
        public bool? CommitFlag { get; set; } = true;
        public bool? ShiftFlag { get; set; } = false;
        public DateTime? ShiftTime { get; set; }
        public bool HasExchanged { get; set; }
        public string TheTicketDate { get; set; }
        public int? ParkId { get; set; }
        public string ParkName { get; set; }
        public Guid SyncCode { get; set; } = Guid.NewGuid();

        public TicketType TicketType { get; set; }

        public virtual ICollection<TicketGround> TicketGrounds { get; set; }
        public virtual ICollection<TicketGroundCache> TicketGroundCaches { get; set; }
        public virtual ICollection<TicketSaleBuyer> TicketSaleBuyers { get; set; }
        public virtual ICollection<TicketSaleSeat> TicketSaleSeats { get; set; }
        public virtual ICollection<TicketSaleGroundSharing> TicketSaleGroundSharings { get; set; }

        public void SetTicMoney(int personNum, decimal? ticPrice)
        {
            TicPrice = ticPrice;
            TicMoney = personNum * ticPrice;
        }

        public void SetReaMoney(int personNum, decimal? reaPrice)
        {
            ReaPrice = reaPrice;
            WbReaPrice = reaPrice * CurrencyRate;
            ReaMoney = personNum * reaPrice;
            WbReaMoney = personNum * WbReaPrice;
        }

        public void SetPrintMoney(int personNum, decimal? printPrice)
        {
            PrintPrice = printPrice;
            PrintMoney = personNum * printPrice;
        }

        public void Pay(int? payTypeId, string payTypeName)
        {
            PayFlag = true;
            PayTypeId = payTypeId;
            PayTypeName = payTypeName;
        }

        public void BindTicket(string ticketCode, string cardNo)
        {
            BindFlag = true;
            TicketCode = ticketCode;
            CardNo = cardNo;
        }

        public void Renew(string etime)
        {
            if (TicketGrounds.IsNullOrEmpty())
            {
                throw new ArgumentNullException("TicketGrounds");
            }

            if (TicketStatusId == TicketStatus.过期)
            {
                TicketStatusId = TicketStatus.已用;
                TicketStatusName = TicketStatusId.ToString();
            }
            ValidFlag = true;
            ValidFlagName = "有效";

            foreach (var item in TicketGrounds)
            {
                item.Renew(TicketStatusId, etime);
            }

            if (TicketGroundCaches.IsNullOrEmpty())
            {
                foreach (var item in TicketGrounds)
                {
                    TicketGroundCaches.Add(item.MapToTicketGroundCache());
                }
            }
            else
            {
                foreach (var item in TicketGroundCaches)
                {
                    item.Renew(TicketStatusId, etime);
                }
            }
        }

        public void InValid()
        {
            ValidFlag = false;
            ValidFlagName = "无效";
            TicketStatusId = TicketStatus.作废;
            TicketStatusName = TicketStatus.作废.ToString();
            SurplusNum = 0;
        }

        public void ValidateCancelOrder()
        {
            if (TicketStatusId != TicketStatus.已售)
            {
                throw new UserFriendlyException($"票号：{TicketCode}{TicketStatusName}，不能取消");
            }

            if (HasExchanged || PrintNum > 0)
            {
                throw new UserFriendlyException($"票号：{TicketCode}已取票，不能取消");
            }
        }

        public void Consume(int consumeNum, bool isCheckByNum, bool validFlag)
        {
            if (isCheckByNum)
            {
                if (SurplusNum <= 0)
                {
                    return;
                }

                SurplusNum = Math.Max(SurplusNum.Value - consumeNum, 0);
            }

            TicketStatusId = TicketStatus.已用;
            TicketStatusName = TicketStatusId.ToString();
            ValidFlag = validFlag;
            ValidFlagName = validFlag ? "有效" : "无效";
        }

        public int GetCheckNum()
        {
            return TotalNum.Value / PersonNum.Value;
        }

        public void Print()
        {
            if (!PrintNum.HasValue)
            {
                PrintNum = 0;
            }

            PrintNum++;
        }
    }
}
