using System.Collections.Generic;

namespace Egoal.Invoice
{
    public class InvoiceRequest
    {
        public InvoiceRequest()
        {
            Items = new List<InvoiceItem>();
        }

        /// <summary>
        /// 发票请求流水号（业务订单号）
        /// </summary>
        public string FPQQLSH { get; set; }

        /// <summary>
        /// 开票类型（0：蓝字发票，1：红字发票）
        /// </summary>
        public string KPLX { get; set; } = "0";

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
        /// 销售方手机号
        /// </summary>
        public string XSF_SJH { get; set; }

        /// <summary>
        /// 销售方电子邮箱
        /// </summary>
        public string XSF_DZYX { get; set; }

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
        /// 购买方手机号
        /// </summary>
        public string GMF_SJH { get; set; }

        /// <summary>
        /// 购买方电子邮箱
        /// </summary>
        public string GMF_DZYX { get; set; }

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

        /// <summary>
        /// 行业类型（0：商业，1：其它）
        /// </summary>
        public string HYLX { get; set; } = "0";

        /// <summary>
        /// 微信openId
        /// </summary>
        public string WX_OPENID { get; set; }

        public List<InvoiceItem> Items { get; set; }
    }
}
