namespace Egoal.Invoice.GuangDongBaiWangJiuBin
{
    public class BaiWangOptions : InvoiceOptions
    {
        /// <summary>
        /// 销售方纳税人密钥
        /// </summary>
        public string XSF_NSRMY { get; set; }

        /// <summary>
        /// 销售方注册码
        /// </summary>
        public string XSF_SN { get; set; }

        /// <summary>
        /// 应用秘钥
        /// </summary>
        public string ACCESS_TOKEN { get; set; }

        /// <summary>
        /// 电子发票下载地址
        /// </summary>
        public string KP_DownURL { get; set; }

        /// <summary>
        /// 开票接口地址
        /// </summary>
        public string KP_ServiceUrl { get; set; }
    }
}
