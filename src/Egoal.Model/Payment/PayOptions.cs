namespace Egoal.Payment
{
    public class PayOptions
    {
        /// <summary>
        /// 未支付订单过期时间（单位分钟）
        /// </summary>
        public int WechatTimeOutOrderCancelTime { get; set; } = 5;

        /// <summary>
        /// 微信购票支付方式
        /// </summary>
        public int WxSalePayTypeId { get; set; } = DefaultPayType.微信支付;
    }
}
