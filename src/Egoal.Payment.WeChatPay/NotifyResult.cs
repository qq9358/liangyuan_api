using Egoal.Extensions;
using Egoal.WeChat;

namespace Egoal.Payment.WeChatPay
{
    public class NotifyResult : ResultBase
    {
        /// <summary>
        /// 签名类型
        /// 目前支持HMAC-SHA256和MD5，默认为MD5
        /// </summary>
        public string sign_type { get; set; }

        /// <summary>
        /// 设备号
        /// </summary>
        public string device_info { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 是否关注公众账号
        /// Y-关注，N-未关注
        /// </summary>
        public string is_subscribe { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public string trade_type { get; set; }

        /// <summary>
        /// 付款银行
        /// </summary>
        public string bank_type { get; set; }

        /// <summary>
        /// 标价金额
        /// </summary>
        public decimal total_fee { get; set; }

        /// <summary>
        /// 应结订单金额
        /// 当订单使用了免充值型优惠券后返回该参数，应结订单金额=订单金额-免充值优惠券金额
        /// </summary>
        public decimal? settlement_total_fee { get; set; }

        /// <summary>
        /// 标价币种
        /// </summary>
        public string fee_type { get; set; }

        /// <summary>
        /// 现金支付金额
        /// </summary>
        public decimal cash_fee { get; set; }

        /// <summary>
        /// 现金支付币种
        /// </summary>
        public string cash_fee_type { get; set; }

        /// <summary>
        /// 代金券金额
        /// </summary>
        public decimal? coupon_fee { get; set; }

        /// <summary>
        /// 代金券使用数量
        /// </summary>
        public int? coupon_count { get; set; }

        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 附加数据 
        /// </summary>
        public string attach { get; set; }

        /// <summary>
        /// 支付完成时间
        /// </summary>
        public string time_end { get; set; }

        public NotifyInput ToNotifyInput()
        {
            var input = new NotifyInput();
            input.AppId = appid;
            input.MerchantNo = mch_id;
            input.DeviceInfo = device_info;
            input.OpenId = openid;
            input.IsSubscribe = is_subscribe == "Y";
            input.TradeType = trade_type;
            input.BankType = bank_type;
            input.TotalFee = total_fee / 100M;
            input.FeeType = fee_type;
            input.TransactionId = transaction_id;
            input.ListNo = out_trade_no;
            input.Attach = attach;
            input.PayTime = time_end.ToDateTime(WeChatOptions.DateTimeFormat);

            return input;
        }
    }
}
