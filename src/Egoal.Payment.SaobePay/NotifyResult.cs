using Egoal.Extensions;
using System;
using System.Text;

namespace Egoal.Payment.SaobePay
{
    public class NotifyResult : ResultBase
    {
        public string total_fee { get; set; }
        public string out_trade_no { get; set; }
        public string user_id { get; set; }
        public string pay_trace { get; set; }
        public string pay_time { get; set; }
        public string end_time { get; set; }
        public string channel_trade_no { get; set; }
        public string attach { get; set; }
        public string receipt_fee { get; set; }

        protected override string ToUrl()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("return_code=").Append(return_code).Append("&");
            sb.Append("return_msg=").Append(return_msg).Append("&");
            sb.Append("result_code=").Append(result_code).Append("&");
            sb.Append("pay_type=").Append(pay_type).Append("&");
            sb.Append("user_id=").Append(user_id).Append("&");
            sb.Append("merchant_name=").Append(merchant_name).Append("&");
            sb.Append("merchant_no=").Append(merchant_no).Append("&");
            sb.Append("terminal_id=").Append(terminal_id).Append("&");
            sb.Append("terminal_trace=").Append(terminal_trace).Append("&");
            sb.Append("terminal_time=").Append(terminal_time).Append("&");
            sb.Append("total_fee=").Append(total_fee).Append("&");
            sb.Append("end_time=").Append(end_time).Append("&");
            sb.Append("out_trade_no=").Append(out_trade_no).Append("&");
            sb.Append("channel_trade_no=").Append(channel_trade_no).Append("&");
            sb.Append("attach=").Append(attach);

            return sb.ToString();
        }

        public NotifyInput ToNotifyInput()
        {
            var input = new NotifyInput();
            input.MerchantNo = merchant_no;
            input.DeviceInfo = terminal_id;
            input.OpenId = user_id;
            input.TotalFee = Convert.ToDecimal(total_fee) / 100;
            input.TransactionId = out_trade_no;
            input.SubTransactionId = channel_trade_no;
            input.ListNo = terminal_trace;
            input.Attach = attach;
            input.PayTime = end_time.ToDateTime(SaobePayOptions.DateTimeFormat);
            input.SubPayTypeId = pay_type;

            return input;
        }
    }
}
