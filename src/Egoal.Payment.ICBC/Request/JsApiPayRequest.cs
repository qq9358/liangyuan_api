namespace Egoal.Payment.IcbcPay.Request
{
    /// <summary>
    /// 公众号聚合支付
    /// </summary>
    public class JsApiPayRequest
    {
        /// <summary>
        /// 接口号，目前仅支持上送1.0.0.1
        /// </summary>
        public string interface_version { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string mer_id { get; set; }

        /// <summary>
        /// 渠道商号；商户通过渠道商接入时必送。目前暂不支持上送。
        /// </summary>
        //public string channel_id { get; set; }

        /// <summary>
        /// 第三方应用ID；商户在微信公众号内接入时必送，上送微信分配的公众账号ID；商户通过支付宝生活号接入时必送，上送支付宝分配的应用ID。目前暂不支持上送。
        /// </summary>
        public string tp_app_id { get; set; }

        /// <summary>
        /// 第三方用户标识；商户在微信公众号/支付宝生活号内接入时必送，上送用户在商户appid下的唯一标识。目前暂不支持上送。
        /// </summary>
        public string tp_open_id { get; set; }

        /// <summary>
        /// 商户订单号；需保证商户系统唯一
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 交易类型。用于区分交易场景为线上支付还是线下支付，对应数据字典：OfflinePay-线下支付，OnlinePay-线上支付。商户需按实际交易场景上送，如上送错误可能影响后续交易进行；比如线上支付场景，上送OfflinePay
        /// -线下支付，使用微信支付时，微信会对实际交易场景进行检查，一旦发现不符，微信则会拒绝对应请求。
        /// </summary>
        public string tran_type { get; set; }

        /// <summary>
        /// 交易提交时间，格式为：YYYYMMDDHHmmss
        /// </summary>
        public string order_date { get; set; }

        /// <summary>
        /// 交易过期日期，格式为：YYYYMMDDHHmmss
        /// </summary>
        public string end_time { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string goods_body { get; set; }

        /// <summary>
        /// 商品详情
        /// </summary>
        public string goods_detail { get; set; }

        /// <summary>
        /// 附加数据。商户可上送定制信息（如商户会话ID、终端设备编号等），在支付结束后的支付结果通知报文该字段原样返回，该字段可以在对账单中体现
        /// </summary>
        public string attach { get; set; }

        /// <summary>
        /// 总金额（单位：分）
        /// </summary>
        public string order_amount { get; set; }

        /// <summary>
        /// 终端IP
        /// </summary>
        public string spbill_create_ip { get; set; }

        /// <summary>
        /// 分期期数。目前仅支持1-不分期
        /// </summary>
        public string install_times { get; set; }

        /// <summary>
        /// 商家提示。目前暂无处理，后续可用于在交易页面回显给客户
        /// </summary>
        public string mer_hint { get; set; }

        /// <summary>
        /// 支付成功回显页面。支付成功后，客户端引导跳转至该页面显示
        /// </summary>
        public string return_url { get; set; }

        /// <summary>
        /// 支付方式限定；上送“no_credit”表示不支持信用卡支付；不上送或上送为空表示无限制；上送“no_balance”表示仅支持银行卡支付（需要微信审批通过后可以接入）
        /// </summary>
        public string pay_limit { get; set; }

        /// <summary>
        /// 支付结果通知地址；上送互联网可访问的URL地址（必须包含协议）；应支持受理同一笔订单的的多次通知场景
        /// </summary>
        public string notify_url { get; set; }

        /// <summary>
        /// 通知类型，表示交易处理完成后把交易结果通知商户的处理模式。取值“HS”：在交易完成后将通知信息，主动发送给商户，发送地址为notify_url指定地址；取值“AG”：在交易完成后不通知商户
        /// </summary>
        public string notify_type { get; set; }

        /// <summary>
        /// 结果发送类型，通知方式为HS时有效。取值“0”：无论支付成功或者失败，银行都向商户发送交易通知信息；取值“1”，银行只向商户发送交易成功通知信息
        /// </summary>
        public string result_type { get; set; }

        /// <summary>
        /// 备用字段1，后续扩展使用
        /// </summary>
        public string backup1 { get; set; }

        /// <summary>
        /// 备用字段2，后续扩展使用
        /// </summary>
        public string backup2 { get; set; }

        /// <summary>
        /// 备用字段3，后续扩展使用
        /// </summary>
        public string backup3 { get; set; }

        /// <summary>
        /// 备用字段4，后续扩展使用
        /// </summary>
        public string backup4 { get; set; }
    }
}
