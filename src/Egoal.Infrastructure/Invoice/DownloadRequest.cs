using System;

namespace Egoal.Invoice
{
    public class DownloadRequest
    {
        /// <summary>
        /// 销售方纳税人识别号
        /// </summary>
        public string XSF_NSRSBH { get; set; }

        /// <summary>
        /// 发票请求流水号（业务订单号）
        /// </summary>
        public string FPQQLSH { get; set; }

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
        public decimal JSHJ { get; set; }

        /// <summary>
        /// 开票日期
        /// </summary>
        public DateTime KPRQ { get; set; }
    }
}
