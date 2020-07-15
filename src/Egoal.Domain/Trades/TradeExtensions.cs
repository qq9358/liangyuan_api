namespace Egoal.Trades
{
    public static class TradeExtensions
    {
        public static Trade CopyTo(this Trade originalTrade, Trade trade)
        {
            trade.TradeTypeTypeId = originalTrade.TradeTypeTypeId;
            trade.TradeTypeId = originalTrade.TradeTypeId;
            trade.TradeTypeName = originalTrade.TradeTypeName;
            trade.TradeSource = originalTrade.TradeSource;
            trade.PayTypeId = originalTrade.PayTypeId;
            trade.PayTypeName = originalTrade.PayTypeName;
            trade.PayFlag = originalTrade.PayFlag;
            trade.CurrencyId = originalTrade.CurrencyId;
            trade.CurrencyName = originalTrade.CurrencyName;
            trade.CurrencyRate = originalTrade.CurrencyRate;
            trade.SalesmanId = originalTrade.SalesmanId;
            trade.SalesmanName = originalTrade.SalesmanName;
            trade.ApproverId = originalTrade.ApproverId;
            trade.ApproverName = originalTrade.ApproverName;
            trade.Mobile = originalTrade.Mobile;
            trade.MemberId = originalTrade.MemberId;
            trade.MemberName = originalTrade.MemberName;
            trade.CustomerId = originalTrade.CustomerId;
            trade.CustomerName = originalTrade.CustomerName;
            trade.GuiderId = originalTrade.GuiderId;
            trade.GuiderName = originalTrade.GuiderName;
            trade.ManagerId = originalTrade.ManagerId;
            trade.ManagerName = originalTrade.ManagerName;
            trade.AreaId = originalTrade.AreaId;
            trade.AreaName = originalTrade.AreaName;
            trade.KeYuanTypeId = originalTrade.KeYuanTypeId;
            trade.KeYuanTypeName = originalTrade.KeYuanTypeName;
            trade.OrderListNo = originalTrade.OrderListNo;
            trade.ThirdPartyPlatformId = originalTrade.ThirdPartyPlatformId;
            trade.ThirdPartyPlatformOrderId = originalTrade.ThirdPartyPlatformOrderId;
            trade.Ota = originalTrade.Ota;
            trade.StatFlag = originalTrade.StatFlag;

            return trade;
        }

        public static TradeDetail MapToTradeDetail(this Trade trade)
        {
            var tradeDetail = new TradeDetail();
            tradeDetail.TradeId = trade.Id;
            tradeDetail.ListNo = trade.ListNo;
            tradeDetail.TradeTypeTypeId = trade.TradeTypeTypeId;
            tradeDetail.TradeTypeId = trade.TradeTypeId;
            tradeDetail.TradeTypeName = trade.TradeTypeName;
            tradeDetail.CashierId = trade.CashierId;
            tradeDetail.StatFlag = trade.StatFlag;
            tradeDetail.BdFlag = trade.BdFlag;
            tradeDetail.Cdate = trade.Cdate;
            tradeDetail.Cweek = trade.Cweek;
            tradeDetail.Cmonth = trade.Cmonth;
            tradeDetail.Cquarter = trade.Cquarter;
            tradeDetail.Cyear = trade.Cyear;
            tradeDetail.Ctp = trade.Ctp;
            tradeDetail.Ctime = trade.Ctime;
            tradeDetail.ParkId = trade.ParkId;

            return tradeDetail;
        }

        public static PayDetail MapToPayDetail(this Trade trade)
        {
            var payDetail = new PayDetail();
            payDetail.TradeId = trade.Id;
            payDetail.ListNo = trade.ListNo;
            payDetail.PayMoney = trade.TotalMoney;
            payDetail.PayTypeId = trade.PayTypeId;
            payDetail.PayTypeName = trade.PayTypeName;
            payDetail.PayFlag = trade.PayFlag;
            payDetail.WbPayMoney = trade.WbTotalMoney;
            payDetail.CustomerId = trade.CustomerId;
            payDetail.StatFlag = trade.StatFlag;
            payDetail.BdFlag = trade.BdFlag;
            payDetail.Ctime = trade.Ctime;
            payDetail.Cdate = trade.Cdate;
            payDetail.CashierId = trade.CashierId;
            payDetail.ParkId = trade.ParkId;

            return payDetail;
        }
    }
}
