using System.Security.Cryptography.X509Certificates;

namespace Egoal.WeChat
{
    public class WeChatOptions
    {
        public const string DateTimeFormat = "yyyyMMddHHmmss";

        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string WxAppID { get; set; }

        /// <summary>
        /// 小程序ID
        /// </summary>
        public string WxMiniprogramAppID { get; set; } = "wx6b88b58124b3f2b3";

        /// <summary>
        /// 商户号（必须配置）
        /// </summary>
        public string WxMch_ID { get; set; }

        /// <summary>
        /// 商户支付密钥，参考开户邮件设置（必须配置）
        /// </summary>
        public string WxApiKey { get; set; }

        /// <summary>
        /// 公众帐号secert
        /// </summary>
        public string WxAppsecret { get; set; }

        /// <summary>
        /// 小程序appSecret
        /// </summary>
        public string WxMiniprogramAppSecret { get; set; } = "62759de6366f4c519ba764848100a312";

        /// <summary>
        /// 接口调用凭据
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 调用微信JS接口的临时票据
        /// </summary>
        public string JsApiTicket { get; set; }

        /// <summary>
        /// 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        /// </summary>
        public string WxCertPath { get; set; }

        /// <summary>
        /// 证书密钥
        /// </summary>
        public string WxCertPassword { get; set; }

        /// <summary>
        /// 微信证书字符串
        /// </summary>
        public string WxCertStr { get; set; }

        public X509Certificate2 SslCert { get; set; }

        /// <summary>
        /// 微信购票地址
        /// </summary>
        public string WxSaleUrl { get; set; }

        /// <summary>
        /// 微信公众号地址
        /// </summary>
        public string WxSubscribeUrl { get; set; }

        /// <summary>
        /// 支付接口地址
        /// </summary>
        public string PayUrl { get; set; } = "https://api.mch.weixin.qq.com";

        /// <summary>
        /// 支付结果通知回调url，用于商户接收支付结果
        /// </summary>
        public string NotifyUrl { get; set; }

        /// <summary>
        /// 商户系统后台机器IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 代理服务器设置
        /// 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        /// </summary>
        public string ProxyUrl { get; set; }

        /// <summary>
        /// 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        /// </summary>
        public int ReportLevenl { get; set; } = 1;

        /// <summary>
        /// 最小支付过期时间（单位分钟）
        /// </summary>
        public int MinExpireMinutes { get; set; } = 5;
    }
}
