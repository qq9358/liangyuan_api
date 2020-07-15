using Egoal.Trades;

namespace Egoal.Orders
{
    public static class OrderExtensions
    {
        public static TradeSource ToTradeSource(this OrderType orderType)
        {
            switch (orderType)
            {
                case OrderType.微信订票:
                    {
                        return TradeSource.微信;
                    }
                case OrderType.电话订票:
                    {
                        return TradeSource.电话订票;
                    }
                case OrderType.第三方订票:
                    {
                        return TradeSource.第三方;
                    }
                case OrderType.网上订票:
                    {
                        return TradeSource.网上;
                    }
                default:
                    {
                        return TradeSource.本地;
                    }
            }
        }

        public static OrderDetail MapToOrderDetail(this Order order)
        {
            var orderDetail = new OrderDetail();
            orderDetail.OrderTypeId = order.OrderTypeId;
            orderDetail.TicketStime = order.Etime;
            orderDetail.MemberId = order.MemberId;
            orderDetail.MemberName = order.MemberName;
            orderDetail.CustomerId = order.CustomerId;
            orderDetail.CustomerName = order.CustomerName;
            orderDetail.GuiderId = order.GuiderId;
            orderDetail.GuiderName = order.GuiderName;
            orderDetail.Cdate = order.Cdate;
            orderDetail.Cweek = order.Cweek;
            orderDetail.Cmonth = order.Cmonth;
            orderDetail.Cquarter = order.Cquarter;
            orderDetail.Cyear = order.Cyear;
            orderDetail.Ctp = order.Ctp;
            orderDetail.CID = order.CID;
            orderDetail.CTime = order.CTime;
            orderDetail.ParkId = order.ParkId;
            orderDetail.ParkName = order.ParkName;

            return orderDetail;
        }
    }
}
