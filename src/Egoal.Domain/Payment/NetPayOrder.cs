using Egoal.Domain.Entities;
using Egoal.Extensions;
using System;

namespace Egoal.Payment
{
    public class NetPayOrder : Entity<long>
    {
        public string ListNo { get; set; }
        public int? PayTypeId { get; set; }
        public NetPayType? NetPayTypeId { get; set; }
        public string NetPayTypeName { get; set; }
        public string SubPayTypeId { get; set; }
        public OnlinePayTradeType? OnlinePayTradeType { get; set; }
        public string TransactionId { get; set; }
        public string SubTransactionId { get; set; }
        public decimal TotalFee { get; set; }
        public NetPayOrderStatus OrderStatusId { get; set; }
        public string OrderStatusName { get; set; }
        public DateTime? PayTime { get; set; }
        public string BankType { get; set; }
        public string ErrorCode { get; set; }
        public string PayArgs { get; set; }
        public string JsApiPayArgs { get; set; }
        public string Pcid { get; set; }
        public bool? CommitFlag { get; set; } = true;
        public DateTime Ctime { get; set; } = DateTime.Now;
        public DateTime? Mtime { get; set; }

        public void Close()
        {
            OrderStatusId = NetPayOrderStatus.已关闭;
            OrderStatusName = OrderStatusId.ToString();
            ClearPayArgs();
            Mtime = DateTime.Now;
        }

        public void ClearPayArgs()
        {
            PayArgs = null;
            JsApiPayArgs = null;
        }

        public bool IsPayResultUnknown()
        {
            return OrderStatusId.IsIn(NetPayOrderStatus.未支付, NetPayOrderStatus.用户支付中);
        }

        public bool AllowPay()
        {
            return OrderStatusId.IsIn(NetPayOrderStatus.未支付, NetPayOrderStatus.支付失败);
        }

        public bool IsNetPay()
        {
            return PayTypeId.HasValue && DefaultPayType.IsNetPay(PayTypeId.Value);
        }
    }
}
