using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Egoal.Payment.SaobePay
{
    public class ResultBase : ReturnBase
    {
        public string result_code { get; set; }
        public string pay_type { get; set; }
        public string merchant_name { get; set; }
        public string merchant_no { get; set; }
        public string terminal_id { get; set; }
        public string terminal_trace { get; set; }
        public string terminal_time { get; set; }

        protected override string ToUrl()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToUrl()).Append("&");
            sb.Append("result_code=").Append(result_code).Append("&");
            sb.Append("pay_type=").Append(pay_type).Append("&");
            sb.Append("merchant_name=").Append(merchant_name).Append("&");
            sb.Append("merchant_no=").Append(merchant_no).Append("&");
            sb.Append("terminal_id=").Append(terminal_id).Append("&");
            sb.Append("terminal_trace=").Append(terminal_trace).Append("&");
            sb.Append("terminal_time=").Append(terminal_time);

            return sb.ToString();
        }
    }
}
