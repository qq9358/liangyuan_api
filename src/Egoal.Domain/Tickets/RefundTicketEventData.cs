using Egoal.Events.Bus;
using System;
using System.Collections.Generic;

namespace Egoal.Tickets
{
    public class RefundTicketEventData : EventData
    {
        public Guid OriginalTradeId { get; set; }
        public Guid TradeId { get; set; }
        public string RefundListNo { get; set; }
        public string PayListNo { get; set; }
        public int PayTypeId { get; set; }
        public decimal TotalMoney { get; set; }
        public string RefundReason { get; set; }
        public List<RefundTicketItem> Items { get; set; }

        public class RefundTicketItem
        {
            public TicketSale OriginalTicketSale { get; set; }
            public long TicketId { get; set; }
            public int RefundQuantity { get; set; }

            /// <summary>
            /// 退票之后的剩余数量
            /// </summary>
            public int SurplusQuantityAfterRefund { get; set; }
        }
    }
}
