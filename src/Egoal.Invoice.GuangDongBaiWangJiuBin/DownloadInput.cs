using Egoal.AutoMapper;
using System.Text;

namespace Egoal.Invoice.GuangDongBaiWangJiuBin
{
    [AutoMapFrom(typeof(DownloadRequest))]
    public class DownloadInput : IRequestBody
    {
        /// <summary>
        /// 销售方纳税人识别号
        /// </summary>
        public string XSF_NSRSBH { get; set; }

        /// <summary>
        /// 应用秘钥
        /// </summary>
        public string ACCESS_TOKEN { get; set; }

        /// <summary>
        /// 发票代码
        /// </summary>
        public string FP_DM { get; set; }

        /// <summary>
        /// 发票号码
        /// </summary>
        public string FP_HM { get; set; }

        /// <summary>
        /// 价税合计 = 合计金额 + 合计税额
        /// </summary>
        public string JSHJ { get; set; }

        /// <summary>
        /// 开票日期（yyyyMMddHHmmss）
        /// </summary>
        public string KPRQ { get; set; }

        public string ToXml(Encoding encoding, bool encrypt)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<user lxdm=\"用户类型\">");
            xml.Append($"<name>{XSF_NSRSBH}</name>");
            xml.Append($"<access_token>{ACCESS_TOKEN}</access_token>");
            xml.Append("</user>");
            xml.Append("<COMMON_FPXX_CFDZS size=\"1\">");
            xml.Append("<COMMON_FPXX_CFDZ>");
            xml.Append($"<FP_DM>{FP_DM}</FP_DM>");
            xml.Append($"<FP_HM>{FP_HM}</FP_HM>");
            xml.Append($"<JSHJ>{JSHJ}</JSHJ>");
            xml.Append($"<KPRQ>{KPRQ}</KPRQ>");
            xml.Append("</COMMON_FPXX_CFDZ>");
            xml.Append("</COMMON_FPXX_CFDZS>");

            return xml.ToString();
        }
    }
}
