using System.Text;

namespace Egoal.Invoice.GuangDongBaiWangJiuBin
{
    public class QueryInput : IRequestBody
    {
        /// <summary>
        /// 发票请求流水号（业务订单号）
        /// </summary>
        public string FPQQLSH { get; set; }

        public string ToXml(Encoding encoding, bool encrypt)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<REQUEST_COMMON_FPCX class=\"REQUEST_COMMON_FPCX\">");
            xml.Append($"<FPQQLSH>{FPQQLSH}</FPQQLSH>");
            xml.Append("</REQUEST_COMMON_FPCX>");

            return xml.ToString();
        }
    }
}
