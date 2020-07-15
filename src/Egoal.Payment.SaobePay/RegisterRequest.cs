using Egoal.Cryptography;
using System.Text;

namespace Egoal.Payment.SaobePay
{
    public class RegisterRequest
    {
        public string pay_ver { get; set; }
        public string service_id { get; set; }
        public string merchant_no { get; set; }
        public string terminal_id { get; set; }
        public string terminal_trace { get; set; }
        public string terminal_time { get; set; }
        public string key_sign { get; private set; }

        public void MakeSign()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("pay_ver=").Append(pay_ver).Append("&");
            sb.Append("service_id=").Append(service_id).Append("&");
            sb.Append("merchant_no=").Append(merchant_no).Append("&");
            sb.Append("terminal_id=").Append(terminal_id).Append("&");
            sb.Append("terminal_trace=").Append(terminal_trace).Append("&");
            sb.Append("terminal_time=").Append(terminal_time);

            key_sign = MD5Helper.Encrypt(sb.ToString());
        }
    }
}
