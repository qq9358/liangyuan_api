using Egoal.Extensions;
using System.Text;

namespace Egoal.Payment.SaobePay
{
    public class ReverseResult : ResultBase
    {
        public string recall { get; set; }

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
            sb.Append("terminal_time=").AppendWithNull(terminal_time).Append("&");
            sb.Append("recall=").AppendWithNull(recall);

            return sb.ToString();
        }

        public ReversePayOutput ToReverseOutput()
        {
            var output = new ReversePayOutput();
            output.Success = result_code == "01";
            output.ShouldRetry = recall == "Y";
            output.ErrorMessage = return_msg;

            return output;
        }
    }
}
