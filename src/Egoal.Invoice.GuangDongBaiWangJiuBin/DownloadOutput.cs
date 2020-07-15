using System.Linq;
using System.Xml.Linq;

namespace Egoal.Invoice.GuangDongBaiWangJiuBin
{
    public class DownloadOutput
    {
        /// <summary>
        /// 销售方纳税人识别号
        /// </summary>
        public string XSF_NSRSBH { get; set; }

        /// <summary>
        /// 发票状态(0000代表有发票，-1代表没有发票)
        /// </summary>
        public string FPZT { get; set; }

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

        /// <summary>
        /// 发票地址
        /// </summary>
        public string FP_URL { get; set; }

        /// <summary>
        /// 返回代码
        /// </summary>
        public string RETURNCODE { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string RETURNMSG { get; set; }

        public static DownloadOutput FromXml(string xml)
        {
            var output = new DownloadOutput();

            XDocument document = XDocument.Parse(xml);
            var business = document.Element("business");

            output.XSF_NSRSBH = business.Element("user").Element("name").Value;

            var fpxx = business.Element("COMMON_FPXX_CFDZS").Elements().First();
            output.FPZT = fpxx.Element("FPZT").Value;
            output.FP_DM = fpxx.Element("FP_DM").Value;
            output.FP_HM = fpxx.Element("FP_HM").Value;
            output.JSHJ = fpxx.Element("JSHJ").Value;
            output.KPRQ = fpxx.Element("KPRQ").Value;
            output.FP_URL = fpxx.Element("FP_URL").Value;

            output.RETURNCODE = business.Element("returnCode").Value;
            output.RETURNMSG = business.Element("returnMsg").Value;

            return output;
        }

        public DownloadResponse ToResponse()
        {
            DownloadResponse response = new DownloadResponse();
            response.FP_URL = FP_URL;

            return response;
        }
    }
}
