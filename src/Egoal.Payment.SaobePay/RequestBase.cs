using Egoal.Cryptography;
using System.Text;

namespace Egoal.Payment.SaobePay
{
    public abstract class RequestBase
    {
        public string pay_ver { get; set; }
        public string pay_type { get; set; }
        public string service_id { get; set; }
        public string merchant_no { get; set; }
        public string terminal_id { get; set; }
        public string terminal_trace { get; set; }
        public string terminal_time { get; set; }
        public string key_sign { get; protected set; }

        public void MakeSign(string key)
        {
            string s = $"{ToUrl()}&access_token={key}";

            key_sign = MD5Helper.Encrypt(s);
        }

        protected virtual string ToUrl()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("pay_ver=").Append(pay_ver).Append("&");
            sb.Append("pay_type=").Append(pay_type).Append("&");
            sb.Append("service_id=").Append(service_id).Append("&");
            sb.Append("merchant_no=").Append(merchant_no).Append("&");
            sb.Append("terminal_id=").Append(terminal_id).Append("&");
            sb.Append("terminal_trace=").Append(terminal_trace).Append("&");
            sb.Append("terminal_time=").Append(terminal_time);

            return sb.ToString();
        }
    }
}
