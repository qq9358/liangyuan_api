using System;

namespace Egoal.Payment
{
    public class NetPayInput
    {
        public string ListNo { get; set; }
        public decimal PayMoney { get; set; }
        public string ProductInfo { get; set; }
        public string ProductId { get; set; }
        public string OpenId { get; set; }
        public string ClientIp { get; set; }
        public string Attach { get; set; }
        public string SubPayTypeId { get; set; }
        public OnlinePayTradeType TradeType { get; set; }
        public string AuthCode { get; set; }
        public DateTime PayStartTime { get; set; }
        public DateTime PayExpireTime { get; set; }
        public string ReturnUrl { get; set; }
        public string WapName { get; set; }
    }
}
