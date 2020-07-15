namespace Egoal.Invoice
{
    public class InvoiceOptions
    {
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
        public string XSF_Email { get; set; }

        /// <summary>
        /// 开票人
        /// </summary>
        public string KPR { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        public string XSF_SPBM { get; set; }

        /// <summary>
        /// 零税率标识（空：非零税率，1：免税，2：不征收，3：普通零税率）
        /// </summary>
        public string SLBS { get; set; }

        /// <summary>
        /// 税率
        /// </summary>
        public decimal XSF_SL { get; set; }
    }
}
