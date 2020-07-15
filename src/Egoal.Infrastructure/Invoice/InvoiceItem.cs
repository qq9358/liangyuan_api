namespace Egoal.Invoice
{
    public class InvoiceItem
    {
        /// <summary>
        /// 发票行性质（0：正常行，1：折扣行，2：被折扣行）
        /// </summary>
        public string FPHXZ { get; set; } = "0";

        /// <summary>
        /// 商品编码
        /// </summary>
        public string SPBM { get; set; }

        /// <summary>
        /// 自行编码
        /// </summary>
        public string ZXBM { get; set; }

        /// <summary>
        /// 优惠政策标识（0：不使用，1：使用）
        /// </summary>
        public string YHZCBS { get; set; } = "0";

        /// <summary>
        /// 零税率标识（空：非零税率，1：免税，2：不征收，3：普通零税率）
        /// </summary>
        public string LSLBS { get; set; }

        /// <summary>
        /// 增值税特殊管理
        /// </summary>
        public string ZZSTSGL { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string XMMC { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string GGXH { get; set; }

        /// <summary>
        /// 计量单位
        /// </summary>
        public string DW { get; set; }

        /// <summary>
        /// 项目数量
        /// </summary>
        public int XMSL { get; set; }

        /// <summary>
        /// 项目单价
        /// </summary>
        public decimal XMDJ { get; set; }

        /// <summary>
        /// 项目金额
        /// </summary>
        public decimal XMJE { get; set; }

        /// <summary>
        /// 税率
        /// </summary>
        public decimal? SL { get; set; }

        /// <summary>
        /// 税额
        /// </summary>
        public decimal SE { get; set; }

        /// <summary>
        /// 销售单价
        /// </summary>
        public decimal RealPrice { get; set; }
    }
}
