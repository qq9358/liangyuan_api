using Egoal.Extensions;
using Egoal.WeChat;

namespace Egoal.Payment.WeChatPay
{
    public class MicroPayResult : ResultBase
    {
        public string device_info { get; set; }
        public string openid { get; set; }
        public string is_subscribe { get; set; }
        public string trade_type { get; set; }
        public string bank_type { get; set; }
        public string fee_type { get; set; }
        public int total_fee { get; set; }
        public int? settlement_total_fee { get; set; }
        public int? coupon_fee { get; set; }
        public string cash_fee_type { get; set; }
        public int cash_fee { get; set; }
        public string transaction_id { get; set; }
        public string out_trade_no { get; set; }
        public string attach { get; set; }
        public string time_end { get; set; }
        public string promotion_detail { get; set; }

        public NetPayOutput ToPayOutput()
        {
            var output = new NetPayOutput();
            output.AppId = appid;
            output.MerchantNo = mch_id;
            output.DeviceInfo = device_info;
            output.OpenId = openid;
            output.IsSubscribe = is_subscribe == "Y";
            output.TradeType = trade_type;
            output.BankType = bank_type;
            output.TotalFee = total_fee / 100M;
            output.FeeType = fee_type;
            output.TransactionId = transaction_id;
            output.ListNo = out_trade_no;
            output.Attach = attach;
            output.PayTime = time_end.ToDateTime(WeChatOptions.DateTimeFormat);
            output.ErrorMessage = return_code == "SUCCESS" ? err_code_des : return_msg;
            output.IsPaid = result_code == "SUCCESS";
            output.IsPaying = err_code.IsIn("SYSTEMERROR", "BANKERROR", "USERPAYING");

            return output;
        }
    }
}
