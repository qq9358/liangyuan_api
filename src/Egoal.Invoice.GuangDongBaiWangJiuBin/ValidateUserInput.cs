using System.Text;

namespace Egoal.Invoice.GuangDongBaiWangJiuBin
{
    public class ValidateUserInput : IRequestBody
    {
        /// <summary>
        /// 销售方纳税人识别号
        /// </summary>
        public string XSF_NSRSBH { get; set; }

        /// <summary>
        /// 销售方注册码
        /// </summary>
        public string XSF_SN { get; set; }

        public string ToXml(Encoding encoding, bool encrypt)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<user lxdm=\"用户类型\">");
            xml.Append($"<name>{XSF_NSRSBH}</name>");
            xml.Append($"<sn>{XSF_SN}</sn>");
            xml.Append("</user>");

            return xml.ToString();
        }
    }
}
