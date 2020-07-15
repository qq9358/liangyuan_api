using Egoal.Tickets.Dto;
using Egoal.Trades;

namespace Egoal.Tickets
{
    public static class RefundTicketInputExtensions
    {
        public static TicketSale MapToTicketSale(this RefundTicketInput input)
        {
            var ticketSale = new TicketSale();
            ticketSale.TradeId = input.TradeId;
            ticketSale.ListNo = input.RefundListNo;
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
            ticketSale.ParkId = input.ParkId;
            ticketSale.ParkName = input.ParkName;

            return ticketSale;
        }

        public static Trade MapToTrade(this RefundTicketInput input)
        {
            var trade = new Trade();
            trade.Id = input.TradeId;
            trade.ListNo = input.RefundListNo;
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
            trade.ParkId = input.ParkId;
            trade.ParkName = input.ParkName;

            return trade;
        }
    }
}
