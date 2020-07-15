using System;

namespace Egoal.Invoice
{
    public class InvoiceResponse
    {
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
        /// 校验码
        /// </summary>
        public string JYM { get; set; }

        /// <summary>
        /// 开票日期
        /// </summary>
        public DateTime? KPRQ { get; set; }

        /// <summary>
        /// 发票地址
        /// </summary>
        public string FP_URL { get; set; }
    }
}
