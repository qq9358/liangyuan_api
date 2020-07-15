using System.Collections.Generic;

namespace Egoal.Payment.Alipay
{
    public class RefundRequest
    {
        public string out_trade_no { get; set; }
        public string trade_no { get; set; }
        public decimal refund_amount { get; set; }
        public string refund_currency { get; set; }
        public string refund_reason { get; set; }
        public string out_request_no { get; set; }
        public string operator_id { get; set; }
        public string store_id { get; set; }
        public string terminal_id { get; set; }
        public List<GoodInfo> goods_detail { get; set; }
        public List<OpenApiRoyaltyDetailInfoPojo> refund_royalty_parameters { get; set; }
        public string org_pid { get; set; }
    }
}
