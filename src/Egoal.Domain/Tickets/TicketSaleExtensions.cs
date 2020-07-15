using Egoal.Extensions;
using Egoal.Trades;
using System;

namespace Egoal.Tickets
{
    public static class TicketSaleExtensions
    {
        public static TicketSale MapToTicketSale(this Trade trade)
        {
            var ticketSale = new TicketSale();
            ticketSale.TradeId = trade.Id;
            ticketSale.ListNo = trade.ListNo;
            ticketSale.PayFlag = trade.PayFlag;
            ticketSale.PayTypeId = trade.PayTypeId;
            ticketSale.PayTypeName = trade.PayTypeName;
            ticketSale.CashierId = trade.CashierId;
            ticketSale.CashierName = trade.CashierName;
            ticketSale.CashPcid = trade.CashPcid;
            ticketSale.CashPcname = trade.CashPcname;
            ticketSale.SalePointId = trade.SalePointId;
            ticketSale.SalePointName = trade.SalePointName;
            ticketSale.SalesmanId = trade.SalesmanId;
            ticketSale.SalesmanName = trade.SalesmanName;
            ticketSale.MemberId = trade.MemberId;
            ticketSale.MemberName = trade.MemberName;
            ticketSale.CustomerId = trade.CustomerId;
            ticketSale.CustomerName = trade.CustomerName;
            ticketSale.GuiderId = trade.GuiderId;
            ticketSale.GuiderName = trade.GuiderName;
            ticketSale.ManagerId = trade.ManagerId;
            ticketSale.ManagerName = trade.ManagerName;
            ticketSale.AreaId = trade.AreaId;
            ticketSale.AreaName = trade.AreaName;
            ticketSale.KeYuanTypeId = trade.KeYuanTypeId;
            ticketSale.KeYuanTypeName = trade.KeYuanTypeName;
            ticketSale.OrderListNo = trade.OrderListNo;
            ticketSale.StatFlag = trade.StatFlag;
            ticketSale.BdFlag = trade.BdFlag;
            ticketSale.Cdate = trade.Cdate;
            ticketSale.Cweek = trade.Cweek;
            ticketSale.Cmonth = trade.Cmonth;
            ticketSale.Cquarter = trade.Cquarter;
            ticketSale.Cyear = trade.Cyear;
            ticketSale.Ctp = trade.Ctp;
            ticketSale.Ctime = trade.Ctime;
            ticketSale.ParkId = trade.ParkId;
            ticketSale.ParkName = trade.ParkName;

            return ticketSale;
        }

        public static TicketSale CopyTo(this TicketSale originalTicketSale, TicketSale ticketSale)
        {
            ticketSale.TicketCode = originalTicketSale.TicketCode;
            ticketSale.CardNo = originalTicketSale.CardNo;
            ticketSale.TdCode = originalTicketSale.TdCode;
            ticketSale.InvoiceCode = originalTicketSale.InvoiceCode;
            ticketSale.InvoiceNo = originalTicketSale.InvoiceNo;
            ticketSale.BindFlag = originalTicketSale.BindFlag;
            ticketSale.YsqTicketCode = originalTicketSale.YsqTicketCode;
            ticketSale.TicketTypeTypeId = originalTicketSale.TicketTypeTypeId;
            ticketSale.TicketTypeId = originalTicketSale.TicketTypeId;
            ticketSale.TicketTypeName = originalTicketSale.TicketTypeName;
            ticketSale.TicketTypeProjectId = originalTicketSale.TicketTypeProjectId;
            ticketSale.TicketTypeProjectTypeId = originalTicketSale.TicketTypeProjectTypeId;
            ticketSale.TicketBindTypeId = originalTicketSale.TicketBindTypeId;
            ticketSale.Tkid = originalTicketSale.Tkid;
            ticketSale.Tkname = originalTicketSale.Tkname;
            ticketSale.Ttid = originalTicketSale.Ttid;
            ticketSale.PriceTypeId = originalTicketSale.PriceTypeId;
            ticketSale.PriceTypeName = originalTicketSale.PriceTypeName;
            ticketSale.DiscountTypeId = originalTicketSale.DiscountTypeId;
            ticketSale.DiscountTypeName = originalTicketSale.DiscountTypeName;
            ticketSale.DiscountRate = originalTicketSale.DiscountRate;
            ticketSale.DiscountApproverId = originalTicketSale.DiscountApproverId;
            ticketSale.DiscountApproverName = originalTicketSale.DiscountApproverName;
            ticketSale.TicPrice = originalTicketSale.TicPrice;
            ticketSale.ReaPrice = originalTicketSale.ReaPrice;
            ticketSale.PrintPrice = originalTicketSale.PrintPrice;
            ticketSale.PayTypeId = originalTicketSale.PayTypeId;
            ticketSale.PayTypeName = originalTicketSale.PayTypeName;
            ticketSale.PayFlag = originalTicketSale.PayFlag;
            ticketSale.WbReaPrice = originalTicketSale.WbReaPrice;
            ticketSale.DrpFlag = originalTicketSale.DrpFlag;
            ticketSale.Stime = originalTicketSale.Stime;
            ticketSale.Etime = originalTicketSale.Etime;
            ticketSale.Sdate = originalTicketSale.Sdate;
            ticketSale.MemberId = originalTicketSale.MemberId;
            ticketSale.MemberName = originalTicketSale.MemberName;
            ticketSale.CustomerId = originalTicketSale.CustomerId;
            ticketSale.CustomerName = originalTicketSale.CustomerName;
            ticketSale.GuiderId = originalTicketSale.GuiderId;
            ticketSale.GuiderName = originalTicketSale.GuiderName;
            ticketSale.ManagerId = originalTicketSale.ManagerId;
            ticketSale.ManagerName = originalTicketSale.ManagerName;
            ticketSale.AreaId = originalTicketSale.AreaId;
            ticketSale.AreaName = originalTicketSale.AreaName;
            ticketSale.KeYuanTypeId = originalTicketSale.KeYuanTypeId;
            ticketSale.KeYuanTypeName = originalTicketSale.KeYuanTypeName;
            ticketSale.OrderListNo = originalTicketSale.OrderListNo;
            ticketSale.OrderDetailId = originalTicketSale.OrderDetailId;
            ticketSale.CertNo = originalTicketSale.CertNo;
            ticketSale.CertTypeId = originalTicketSale.CertTypeId;
            ticketSale.CertTypeName = originalTicketSale.CertTypeName;
            ticketSale.ChangCiId = originalTicketSale.ChangCiId;
            ticketSale.StatFlag = originalTicketSale.StatFlag;
            ticketSale.CheckTypeId = originalTicketSale.CheckTypeId;
            ticketSale.IsRechargeFlag = originalTicketSale.IsRechargeFlag;

            return ticketSale;
        }

        public static TicketGround MapToTicketGround(this TicketSale ticketSale)
        {
            var ticketGround = new TicketGround();
            ticketGround.TicketCode = ticketSale.TicketCode;
            ticketGround.CardNo = ticketSale.CardNo;
            ticketGround.CertNo = ticketSale.CertNo;
            ticketGround.ValidFlag = true;
            ticketGround.TicketStatusId = ticketSale.TicketStatusId;
            ticketGround.FingerStatusId = ticketSale.FingerStatusId;
            ticketGround.PhotoBindFlag = ticketSale.PhotoBindFlag;
            ticketGround.TradeId = ticketSale.TradeId;
            ticketGround.TicketTypeTypeId = ticketSale.TicketTypeTypeId;
            ticketGround.TicketTypeId = ticketSale.TicketTypeId;
            ticketGround.CheckTypeId = ticketSale.CheckTypeId;
            ticketGround.Tkid = ticketSale.Tkid;
            ticketGround.ChangCiId = ticketSale.ChangCiId;
            ticketGround.Stime = ticketSale.Stime;
            ticketGround.Etime = ticketSale.Etime;
            ticketGround.TotalNum = ticketSale.TotalNum;
            ticketGround.SurplusNum = ticketSale.SurplusNum;
            ticketGround.MemberId = ticketSale.MemberId;
            ticketGround.Ctime = ticketSale.Ctime;
            ticketGround.ParkId = ticketSale.ParkId;

            return ticketGround;
        }

        public static TicketGroundCache MapToTicketGroundCache(this TicketGround ticketGround)
        {
            var ticketGroundCache = new TicketGroundCache();
            ticketGroundCache.GroundId = ticketGround.GroundId;
            ticketGroundCache.TicketCode = ticketGround.TicketCode;
            ticketGroundCache.CardNo = ticketGround.CardNo;
            ticketGroundCache.CertNo = ticketGround.CertNo;
            ticketGroundCache.ValidFlag = ticketGround.ValidFlag;
            ticketGroundCache.TicketStatusId = ticketGround.TicketStatusId;
            ticketGroundCache.FingerStatusId = ticketGround.FingerStatusId;
            ticketGroundCache.PhotoBindFlag = ticketGround.PhotoBindFlag;
            ticketGroundCache.TicketId = ticketGround.TicketId;
            ticketGroundCache.TradeId = ticketGround.TradeId;
            ticketGroundCache.TicketTypeTypeId = ticketGround.TicketTypeTypeId;
            ticketGroundCache.TicketTypeId = ticketGround.TicketTypeId;
            ticketGroundCache.CheckTypeId = ticketGround.CheckTypeId;
            ticketGroundCache.Tkid = ticketGround.Tkid;
            ticketGroundCache.ChangCiId = ticketGround.ChangCiId;
            ticketGroundCache.SeatId = ticketGround.SeatId;
            ticketGroundCache.Stime = ticketGround.Stime;
            ticketGroundCache.Etime = ticketGround.Etime;
            ticketGroundCache.TotalNum = ticketGround.TotalNum;
            ticketGroundCache.SurplusNum = ticketGround.SurplusNum;
            ticketGroundCache.CheckOutNum = ticketGround.CheckOutNum;
            ticketGroundCache.GroundPrice = ticketGround.GroundPrice;
            ticketGroundCache.MemberId = ticketGround.MemberId;
            ticketGroundCache.Ctime = ticketGround.Ctime;
            ticketGroundCache.LastIoflag = ticketGround.LastIoflag;
            ticketGroundCache.LastInGateId = ticketGround.LastInGateId;
            ticketGroundCache.LastInCheckTime = ticketGround.LastInCheckTime;
            ticketGroundCache.LastOutGateId = ticketGround.LastOutGateId;
            ticketGroundCache.LastOutCheckTime = ticketGround.LastOutCheckTime;
            ticketGroundCache.CheckTimesByDay = ticketGround.CheckTimesByDay;
            ticketGroundCache.DealFlag = ticketGround.DealFlag;
            ticketGroundCache.TimeoutFlag = ticketGround.TimeoutFlag;
            ticketGroundCache.FirstActiveFlag = ticketGround.FirstActiveFlag;
            ticketGroundCache.SecondActiveFlag = ticketGround.SecondActiveFlag;
            ticketGroundCache.Bid = ticketGround.Bid;
            ticketGroundCache.CommitFlag = ticketGround.CommitFlag;
            ticketGroundCache.ParkId = ticketGround.ParkId;

            return ticketGroundCache;
        }

        public static TicketSaleBuyer MapToTicketSaleBuyer(this TicketSale ticketSale)
        {
            var ticketSaleBuyer = new TicketSaleBuyer();
            ticketSaleBuyer.TradeId = ticketSale.TradeId;
            ticketSaleBuyer.OrderListNo = ticketSale.OrderListNo;
            ticketSaleBuyer.Sdate = ticketSale.Sdate;
            ticketSaleBuyer.Stime = ticketSale.Stime.To<DateTime>();
            ticketSaleBuyer.Etime = ticketSale.Etime.To<DateTime>();
            ticketSaleBuyer.Ctime = ticketSale.Ctime;
            ticketSaleBuyer.ParkId = ticketSale.ParkId;

            return ticketSaleBuyer;
        }

        public static TicketCheck MapToTicketCheck(this TicketSale ticketSale)
        {
            var ticketCheck = new TicketCheck();
            ticketCheck.SaleParkId = ticketSale.ParkId;
            ticketCheck.SaleParkName = ticketSale.ParkName;
            ticketCheck.TradeId = ticketSale.TradeId;
            ticketCheck.ListNo = ticketSale.ListNo;
            ticketCheck.TicketSyncCode = ticketSale.SyncCode;
            ticketCheck.TicketId = ticketSale.Id;
            ticketCheck.TicketCode = ticketSale.TicketCode;
            ticketCheck.CardNo = ticketSale.CardNo;
            ticketCheck.TicketTypeTypeId = ticketSale.TicketTypeTypeId;
            ticketCheck.TicketTypeId = ticketSale.TicketTypeId;
            ticketCheck.TicketTypeName = ticketSale.TicketTypeName;
            ticketCheck.Stime = ticketSale.Stime;
            ticketCheck.Etime = ticketSale.Etime;
            ticketCheck.TotalNum = ticketSale.TotalNum;
            ticketCheck.CashierId = ticketSale.CashierId;
            ticketCheck.CashierName = ticketSale.CashierName;
            ticketCheck.CashPcid = ticketSale.CashPcid;
            ticketCheck.CashPcname = ticketSale.CashPcname;
            ticketCheck.SalePointId = ticketSale.SalePointId;
            ticketCheck.SalePointName = ticketSale.SalePointName;
            ticketCheck.MemberId = ticketSale.MemberId;
            ticketCheck.MemberName = ticketSale.MemberName;
            ticketCheck.CustomerId = ticketSale.CustomerId;
            ticketCheck.CustomerName = ticketSale.CustomerName;
            ticketCheck.GuiderId = ticketSale.GuiderId;
            ticketCheck.GuiderName = ticketSale.GuiderName;
            ticketCheck.StatFlag = ticketSale.StatFlag;
            ticketCheck.BdFlag = ticketSale.BdFlag;
            ticketCheck.ParkId = ticketSale.ParkId;
            ticketCheck.ParkName = ticketSale.ParkName;

            return ticketCheck;
        }
    }
}
