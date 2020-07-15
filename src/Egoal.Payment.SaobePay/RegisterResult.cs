using Egoal.Extensions;
using System.Text;

namespace Egoal.Payment.SaobePay
{
    public class RegisterResult : ReturnBase
    {
        public string result_code { get; set; }
        public string merchant_no { get; set; }
        public string terminal_id { get; set; }
        public string terminal_trace { get; set; }
        public string terminal_time { get; set; }
        public string access_token { get; set; }

        protected override string ToUrl()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToUrl()).Append("&");
            sb.Append("result_code=").AppendWithNull(result_code).Append("&");
            sb.Append("merchant_no=").AppendWithNull(merchant_no).Append("&");
            sb.Append("terminal_id=").AppendWithNull(terminal_id).Append("&");
            sb.Append("terminal_trace=").AppendWithNull(terminal_trace).Append("&");
            sb.Append("terminal_time=").AppendWithNull(terminal_time).Append("&");
            sb.Append("access_token=").AppendWithNull(access_token);

            return sb.ToString();
        }
    }
}
