using Egoal.Extensions;
using System;
using System.Text;

namespace Egoal.Payment.SaobePay
{
    public class MicroPayResult : ResultBase
    {
        public string total_fee { get; set; }
        public string end_time { get; set; }
        public string out_trade_no { get; set; }
        public string channel_trade_no { get; set; }
        public string channel_order_no { get; set; }
        public string user_id { get; set; }
        public string attach { get; set; }
        public string receipt_fee { get; set; }

        protected override string ToUrl()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToUrl()).Append("&");
            sb.Append("total_fee=").AppendWithNull(total_fee).Append("&");
            sb.Append("end_time=").AppendWithNull(end_time).Append("&");
            sb.Append("out_trade_no=").AppendWithNull(out_trade_no);

            return sb.ToString();
        }

        public NetPayOutput ToPayOutput()
        {
            var output = new NetPayOutput();
            output.MerchantNo = merchant_no;
            output.DeviceInfo = terminal_id;
            output.OpenId = user_id;
            output.SubPayTypeId = pay_type;
            output.TotalFee = Convert.ToDecimal(total_fee) / 100;
            output.TransactionId = out_trade_no;
            output.SubTransactionId = channel_trade_no;
            output.ListNo = terminal_trace;
            output.Attach = attach;
            output.PayTime = end_time.ToDateTime(SaobePayOptions.DateTimeFormat);
            output.ErrorMessage = return_msg;
            output.IsPaid = result_code == "01";
            output.IsPaying = result_code == "03";

            return output;
        }
    }
}
