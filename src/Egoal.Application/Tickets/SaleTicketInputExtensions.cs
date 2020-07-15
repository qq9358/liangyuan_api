using Egoal.Orders;
using Egoal.Tickets.Dto;
using Egoal.Trades;

namespace Egoal.Tickets
{
    public static class SaleTicketInputExtensions
    {
        public static TicketSale MapToTicketSale(this SaleTicketInput input)
        {
            var ticketSale = new TicketSale();
            ticketSale.TradeId = input.TradeId;
            ticketSale.ListNo = input.ListNo;
            ticketSale.ChangCiId = input.ChangCiId;
            ticketSale.MemberId = input.MemberId;
            ticketSale.MemberName = input.MemberName;
            ticketSale.CustomerId = input.CustomerId;
            ticketSale.CustomerName = input.CustomerName;
            ticketSale.GuiderId = input.GuiderId;
            ticketSale.GuiderName = input.GuiderName;
            ticketSale.ManagerId = input.ManagerId;
            ticketSale.ManagerName = input.ManagerName;
            ticketSale.AreaId = input.AreaId;
            ticketSale.AreaName = input.AreaName;
            ticketSale.KeYuanTypeId = input.KeYuanTypeId;
            ticketSale.KeYuanTypeName = input.KeYuanTypeName;
            ticketSale.OrderListNo = input.OrderListNo;
            ticketSale.BdFlag = input.BdFlag;
            ticketSale.Cdate = input.Cdate;
            ticketSale.Cweek = input.Cweek;
            ticketSale.Cmonth = input.Cmonth;
            ticketSale.Cquarter = input.Cquarter;
            ticketSale.Cyear = input.Cyear;
            ticketSale.Ctp = input.Ctp;
            ticketSale.Ctime = input.Ctime;
            ticketSale.CashierId = input.CashierId;
            ticketSale.CashierName = input.CashierName;
            ticketSale.CashPcid = input.CashPcid;
            ticketSale.CashPcname = input.CashPcname;
            ticketSale.SalePointId = input.SalePointId;
            ticketSale.SalePointName = input.SalePointName;
            ticketSale.SalesmanId = input.SalesmanId;
            ticketSale.SalesmanName = input.SalesmanName;
            ticketSale.ParkId = input.ParkId;
            ticketSale.ParkName = input.ParkName;

            return ticketSale;
        }

        public static Trade MapToTrade(this SaleTicketInput input)
        {
            var trade = new Trade();
            trade.Id = input.TradeId;
            trade.ListNo = input.ListNo;
            trade.TradeTypeTypeId = input.TradeTypeTypeId;
            trade.TradeTypeId = input.TradeTypeId;
            trade.TradeTypeName = input.TradeTypeName;
            trade.TradeSource = input.TradeSource;
            trade.MemberId = input.MemberId;
            trade.MemberName = input.MemberName;
            trade.CustomerId = input.CustomerId;
            trade.CustomerName = input.CustomerName;
            trade.GuiderId = input.GuiderId;
            trade.GuiderName = input.GuiderName;
            trade.ManagerId = input.ManagerId;
            trade.ManagerName = input.ManagerName;
            trade.AreaId = input.AreaId;
            trade.AreaName = input.AreaName;
            trade.KeYuanTypeId = input.KeYuanTypeId;
            trade.KeYuanTypeName = input.KeYuanTypeName;
            trade.OrderListNo = input.OrderListNo;
            trade.BdFlag = input.BdFlag;
            trade.Cdate = input.Cdate;
            trade.Cweek = input.Cweek;
            trade.Cmonth = input.Cmonth;
            trade.Cquarter = input.Cquarter;
            trade.Cyear = input.Cyear;
            trade.Ctp = input.Ctp;
            trade.Ctime = input.Ctime;
            trade.CashierId = input.CashierId;
            trade.CashierName = input.CashierName;
            trade.CashPcid = input.CashPcid;
            trade.CashPcname = input.CashPcname;
            trade.SalePointId = input.SalePointId;
            trade.SalePointName = input.SalePointName;
            trade.SalesmanId = input.SalesmanId;
            trade.SalesmanName = input.SalesmanName;
            trade.ParkId = input.ParkId;
            trade.ParkName = input.ParkName;

            return trade;
        }

        public static SaleTicketInput MapToSaleTicketInput(this Order order)
        {
            var saleTicketInput = new SaleTicketInput();
            saleTicketInput.ListNo = order.Id;
            saleTicketInput.ChangCiId = order.ChangCiId;
            saleTicketInput.TradeSource = order.OrderTypeId.ToTradeSource();
            saleTicketInput.PayTypeId = order.PayTypeId;
            saleTicketInput.PayTypeName = order.PayTypeName;
            saleTicketInput.PayFlag = order.PayFlag ?? false;
            saleTicketInput.MemberId = order.MemberId;
            saleTicketInput.MemberName = order.MemberName;
            saleTicketInput.CustomerId = order.CustomerId;
            saleTicketInput.CustomerName = order.CustomerName;
            saleTicketInput.GuiderId = order.GuiderId;
            saleTicketInput.GuiderName = order.GuiderName;
            saleTicketInput.AreaId = order.KeYuanAreaId;
            saleTicketInput.KeYuanTypeId = order.KeYuanTypeId;
            saleTicketInput.OrderListNo = order.Id;
            saleTicketInput.ParkId = order.ParkId;
            saleTicketInput.ParkName = order.ParkName;

            return saleTicketInput;
        }
    }
}
