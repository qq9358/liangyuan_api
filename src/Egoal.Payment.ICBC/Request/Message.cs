namespace Egoal.Payment.IcbcPay.Request
{
    /// <summary>
    /// 商品通讯区
    /// </summary>
    public class Message
    {
        /// <summary>
        /// 商品编号，最大长度10
        /// </summary>
        public string goods_id { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string goods_name { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public string goods_num { get; set; }

        /// <summary>
        /// 已含运费金额，选输，以分为单位
        /// </summary>
        public string carriage_amt { get; set; }

        /// <summary>
        /// 商城提示
        /// </summary>
        public string mer_hint { get; set; }

        /// <summary>
        /// 备注，如果希望对订单的有效日期进行限定，此项必须输入类似"20190828101230"的时间串代表8月28日10:12:30之前支付订单有效。若不需要限定，此项送空
        /// </summary>
        public string remark1 { get; set; }

        /// <summary>
        /// 选输
        /// </summary>
        public string remark2 { get; set; }

        /// <summary>
        /// 移动端送空，PC端支付必输。默认"2"。取值范围0、1、2，其中0表示仅允许使用借记卡支付，1表示仅允许使用信用卡支付，2表示借记卡和信用卡都能对订单进行支付
        /// </summary>
        public string credit_type { get; set; }

        /// <summary>
        /// 移动端送空，pc端支付选输，工行在支付页面显示该信息。上送商户网站域名（支持通配符，例如"*.某B2C商城.com"），如果上送，工行为在客户支付订单时，校验商户上送域名与客户跳转工行支付页面之前网站域名的一致性
        /// </summary>
        public string mer_reference { get; set; }

        /// <summary>
        /// 移动端送空，pc端支付选输，工行在支付页面显示该信息。使用IPV4格式。当商户reference项送空时，该项必输。
        /// </summary>
        public string mer_custom_ip { get; set; }

        /// <summary>
        /// 移动端送空，pc端支付选输，工行在支付页面显示该信息。若输入”0“：虚拟商品；”1“：实物商品
        /// </summary>
        public string goods_type { get; set; }

        /// <summary>
        /// 移动端送空，pc端支付选输，工行在支付页面显示该信息。
        /// </summary>
        public string mer_custom_id { get; set; }

        /// <summary>
        /// 移动端卷帘，pc端支付选输，工行在支付页面显示该信息。
        /// </summary>
        public string mer_custom_phone { get; set; }

        /// <summary>
        /// 移动端送空，pc端支付选输，工行在支付页面显示该信息
        /// </summary>
        public string goods_address { get; set; }

        /// <summary>
        /// 移动端送空，pc端支付选输，工行在支付页面显示该信息
        /// </summary>
        public string mer_order_remark { get; set; }

        /// <summary>
        /// 返回商户变量，商户自定义，当返回银行结果时，作为一个隐藏域变量，商户可以用此变量维护session等等。由客户端浏览器支付完成后提交通知结果时是明文传输，商户对此变量需使用BASE64加密
        /// </summary>
        public string mer_var { get; set; }

        /// <summary>
        /// 通知类型，在交易转账处理完成后把交易结果通知商户的处理模式。取值”HS“：在交易完成后实时将通知信息以HTTP协议POST方式，主动发送给商户，发送地址为商户端随订单数据提交的接收工行支付结果的URL;
        /// 取值“AG”：在交易完成后不通知商户。商户需要使用浏览器登录工行的B2C商户服务网站，或者使用工行提供的客户端程序API主动获取通知信息。取值“LS”；在交易完成后实时将通知信息以HTTP协议POST方式，
        /// 主动发送给商户，发送地址为商户随订单数据提交的接收工行支付结果的URL,即表单中的merURL字段，商户响应银行通知时返回取贷链接给工行，如工行未收到商户响应则重复发送通知消息，发送次数由工行参数
        /// 配置。
        /// </summary>
        public string notify_type { get; set; }

        /// <summary>
        /// 结果发送类型，取值“0”：无论支付成功或者失败，银行都向商户发送交易通知信息；取值“1”，银行只向商户发送交易成功的通知信息。只有通知方式为HS时此值有效，如果使用AG方式，可不上送此项但签名
        /// 数据中必须包含此项，取值可为空
        /// </summary>
        public string result_type { get; set; }

        /// <summary>
        /// 卡控身份一致性（便于以后增加校验类型等）0：不进去卡控，选输 默认不进行卡控
        /// </summary>
        public string is_real { get; set; }

        /// <summary>
        /// 证件类型及用户证件号码后6位组成的字符串，最多可有6个身份信息。证件类型与证件号码之间用“：”分割，多组证件号码之间以#分割。证件类型有四种：身份证或户口本：1港澳居民来往内地通行证：C台湾居民
        /// 来往大陆通行证：G护照：B示例：1:236634#1:05381X。选输，如果isreal的值为1时必填
        /// </summary>
        public string subidno { get; set; }

        /// <summary>
        /// 取值1或者2；对应的提示文字提前进行约定。选输，如果isreal的值为1时必填
        /// </summary>
        public string prompt_text { get; set; }

        /// <summary>
        /// 必须合法的URL，如果商户需要在我行支付成功页面提供返回商户功能，则需要提供回调地址
        /// </summary>
        public string return_url { get; set; }

        /// <summary>
        /// 当商户提供的商城取货地址为正常可达时，如该参数为空，则倒计时结束后自动跳转回商城取货地址对应链接，如不上送则默认不自动跳转。
        /// </summary>
        public string auto_refer_sec { get; set; }

        /// <summary>
        /// 选输
        /// </summary>
        public string backup1 { get; set; }

        /// <summary>
        /// 不输，预留字段
        /// </summary>
        public string backup2 { get; set; }

        /// <summary>
        /// 不输，预留字段
        /// </summary>
        public string backup3 { get; set; }

        /// <summary>
        /// 不输预留字段
        /// </summary>
        public string backup4 { get; set; }
    }
}
