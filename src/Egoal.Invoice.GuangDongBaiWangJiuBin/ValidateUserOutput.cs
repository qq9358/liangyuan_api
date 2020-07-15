using System.Xml.Linq;

namespace Egoal.Invoice.GuangDongBaiWangJiuBin
{
    public class ValidateUserOutput
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
        /// 应用秘钥有效截止时间
        /// </summary>
        public string ACCESS_TOKEN_TIME { get; set; }

        /// <summary>
        /// 返回代码
        /// </summary>
        public string RETURNCODE { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string RETURNMSG { get; set; }

        public static ValidateUserOutput FromXml(string xml)
        {
            var output = new ValidateUserOutput();

            XDocument document = XDocument.Parse(xml);
            var business = document.Element("business");

            output.XSF_NSRSBH = business.Element("user").Element("name").Value;
            output.ACCESS_TOKEN = business.Element("user").Element("access_token").Value;
            output.ACCESS_TOKEN_TIME = business.Element("user").Element("access_token_time").Value;
            output.RETURNCODE = business.Element("returnCode").Value;
            output.RETURNMSG = business.Element("returnMsg").Value;

            return output;
        }
    }
}
