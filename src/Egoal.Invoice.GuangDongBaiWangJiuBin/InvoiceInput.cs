using Egoal.AutoMapper;
using Egoal.Cryptography;
using System.Collections.Generic;
using System.Text;

namespace Egoal.Invoice.GuangDongBaiWangJiuBin
{
    [AutoMapFrom(typeof(InvoiceRequest))]
    public class InvoiceInput : IRequestBody
    {
        /// <summary>
        /// 单据编号
        /// </summary>
        public string DJBH { get; set; }

        /// <summary>
        /// 发票请求流水号（业务订单号）
        /// </summary>
        public string FPQQLSH { get; set; }

        /// <summary>
        /// 开票类型（0：蓝字发票，1：红字发票）
        /// </summary>
        public string KPLX { get; set; } = "0";

        /// <summary>
        /// 特殊票种
        /// </summary>
        public string TSPZ { get; set; } = "00";

        /// <summary>
        /// 编码表版本号
        /// </summary>
        public string BMB_BBH { get; set; }

        /// <summary>
        /// 征税方式（0：普通征税，2：差额征税）
        /// </summary>
        public string ZSFS { get; set; } = "0";

        /// <summary>
        /// 销售方纳税人识别号
        /// </summary>
        public string XSF_NSRSBH { get; set; }

        /// <summary>
        /// 销售方名称
        /// </summary>
        public string XSF_MC { get; set; }

        /// <summary>
        /// 销售方地址、电话
        /// </summary>
        public string XSF_DZDH { get; set; }

        /// <summary>
        /// 销售方银行账号
        /// </summary>
        public string XSF_YHZH { get; set; }

        /// <summary>
        /// 销售方移动电话或邮箱
        /// </summary>
        public string XSF_LXFS { get; set; }

        /// <summary>
        /// 购买方纳税人识别号
        /// </summary>
        public string GMF_NSRSBH { get; set; }

        /// <summary>
        /// 购买方名称
        /// </summary>
        public string GMF_MC { get; set; }

        /// <summary>
        /// 购买方地址、电话
        /// </summary>
        public string GMF_DZDH { get; set; }

        /// <summary>
        /// 购买方银行账号
        /// </summary>
        public string GMF_YHZH { get; set; }

        /// <summary>
        /// 购买方移动电话或邮箱
        /// </summary>
        public string GMF_LXFS { get; set; }

        /// <summary>
        /// 开票人
        /// </summary>
        public string KPR { get; set; }

        /// <summary>
        /// 收款人
        /// </summary>
        public string SKR { get; set; }

        /// <summary>
        /// 复核人
        /// </summary>
        public string FHR { get; set; }

        /// <summary>
        /// 原发票代码
        /// </summary>
        public string YFP_DM { get; set; }

        /// <summary>
        /// 原发票号码
        /// </summary>
        public string YFP_HM { get; set; }

        /// <summary>
        /// 价税合计 = 合计金额 + 合计税额
        /// </summary>
        public decimal JSHJ { get; set; }

        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal HJJE { get; set; }

        /// <summary>
        /// 合计税额
        /// </summary>
        public decimal HJSE { get; set; }

        /// <summary>
        /// 扣除额，当ZSFS为2时扣除额为必填项
        /// </summary>
        public decimal? KCE { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string BZ { get; set; }

        public List<XMXX> Items { get; set; }

        public string ToXml(Encoding encoding, bool encrypt)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<body yylxdm=\"1\">");
            xml.Append("<input>");
            xml.Append($"<DJBH>{DJBH}</DJBH>");
            xml.Append($"<FPXML>{BuildContentXml(encoding, encrypt)}</FPXML>");
            xml.Append("</input>");
            xml.Append("</body>");

            return xml.ToString();
        }

        private string BuildContentXml(Encoding encoding, bool encrypt)
        {
            StringBuilder xml = new StringBuilder();
            xml.Append($"<?xml version=\"1.0\" encoding=\"gbk\"?>");
            xml.Append("<business id=\"FPKJ\" comment=\"发票开具\">");
            xml.Append("<HTJS_DZFPKJ class=\"HTJS_DZFPKJ\">");
            xml.Append("<COMMON_FPKJ_FPT class=\"COMMON_FPKJ_FPT\">");
            var properties = GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == "DJBH" || property.Name == "Items") continue;

                var value = property.GetValue(this);
                xml.Append("<").Append(property.Name).Append(">").Append(value ?? string.Empty).Append("</").Append(property.Name).Append(">");
            }
            xml.Append("</COMMON_FPKJ_FPT>");
            xml.Append($"<COMMON_FPKJ_XMXXS class=\"COMMON_FPKJ_XMXX\" size=\"{Items.Count}\">");
            foreach (var item in Items)
            {
                xml.Append(item.ToXml());
            }
            xml.Append("</COMMON_FPKJ_XMXXS>");
            xml.Append("</HTJS_DZFPKJ>");
            xml.Append("</business>");

            if (encrypt)
            {
                return Base64Helper.Encode(xml.ToString(), encoding);
            }
            else
            {
                return xml.ToString();
            }
        }
    }
}
