using Egoal.Extensions;
using Egoal.WeChat;

namespace Egoal.Payment.WeChatPay
{
    public class QueryPayResult : ResultBase
    {

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
        /// 交易状态
        /// SUCCESS—支付成功 
        /// REFUND—转入退款
        /// NOTPAY—未支付
        /// CLOSED—已关闭
        /// REVOKED—已撤销（刷卡支付）
        /// USERPAYING--用户支付中
        /// PAYERROR--支付失败(其他原因，如银行返回失败)
        /// </summary>
        public string trade_state { get; set; }

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

        /// <summary>
        /// 交易状态描述
        /// </summary>
        public string trade_state_desc { get; set; }

        public QueryPayOutput ToQueryPayOutput()
        {
            var output = new QueryPayOutput();
            output.AppId = appid;
            output.MerchantNo = mch_id;
            output.DeviceInfo = device_info;
            output.OpenId = openid;
            output.IsSubscribe = is_subscribe == "Y";
            output.TradeState = trade_state;
            output.TradeType = trade_type;
            output.BankType = bank_type;
            output.TotalFee = total_fee / 100M;
            output.FeeType = fee_type;
            output.TransactionId = transaction_id;
            output.ListNo = out_trade_no;
            output.Attach = attach;
            output.PayTime = time_end.ToDateTime(WeChatOptions.DateTimeFormat);
            output.IsPaid = trade_state?.ToUpper() == "SUCCESS";
            output.IsPaying = trade_state?.ToUpper() == "USERPAYING" || trade_state?.ToUpper() == "NOTPAY" || err_code?.ToUpper() == "SYSTEMERROR";
            output.IsExist = err_code?.ToUpper() != "ORDERNOTEXIST";
            output.IsRefund = trade_state?.ToUpper() == "REFUND";
            output.ErrorMessage = err_code_des;

            return output;
        }
    }
}
