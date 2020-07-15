using Egoal.Extensions;
using System.Text;

namespace Egoal.Payment.SaobePay
{
    public class NativePayResult : ResultBase
    {
        public string total_fee { get; set; }
        public string out_trade_no { get; set; }
        public string qr_code { get; set; }

        protected override string ToUrl()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToUrl()).Append("&");
            sb.Append("total_fee=").AppendWithNull(total_fee).Append("&");
            sb.Append("out_trade_no=").AppendWithNull(out_trade_no).Append("&");
            sb.Append("qr_code=").AppendWithNull(qr_code);

            return sb.ToString();
        }
    }
}
