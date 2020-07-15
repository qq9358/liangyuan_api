using Egoal.Extensions;
using System.Text;

namespace Egoal.Payment.SaobePay
{
    public class CloseOrderResult : ResultBase
    {
        protected override string ToUrl()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("return_code=").AppendWithNull(return_code).Append("&");
            sb.Append("return_msg=").AppendWithNull(return_msg).Append("&");
            sb.Append("result_code=").AppendWithNull(result_code).Append("&");
            sb.Append("pay_type=").AppendWithNull(pay_type).Append("&");
            sb.Append("merchant_no=").AppendWithNull(merchant_no).Append("&");
            sb.Append("terminal_id=").AppendWithNull(terminal_id).Append("&");
            sb.Append("terminal_trace=").AppendWithNull(terminal_trace).Append("&");
            sb.Append("terminal_time=").AppendWithNull(terminal_time);

            return sb.ToString();
        }

        public ClosePayOutput ToCloseOrderOutput()
        {
            var output = new ClosePayOutput();
            output.Success = result_code == "01";

            return output;
        }
    }
}
