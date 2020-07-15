using System;

namespace Egoal.Payment
{
    public class NotifyInput
    {
        public bool PaySuccess { get; set; } = true;
        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string MerchantNo { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string DeviceInfo { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 是否关注公众账号
        /// </summary>
        public bool IsSubscribe { get; set; }
        /// <summary>
        /// 具体支付方式
        /// </summary>
        public string SubPayTypeId { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public string TradeType { get; set; }
        /// <summary>
        /// 付款银行
        /// </summary>
        public string BankType { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal TotalFee { get; set; }
        /// <summary>
        /// 货币种类
        /// </summary>
        public string FeeType { get; set; }
        /// <summary>
        /// 支付单号
        /// </summary>
        public string TransactionId { get; set; }
        /// <summary>
        /// 第三方支付单号
        /// </summary>
        public string SubTransactionId { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public string ListNo { get; set; }
        /// <summary>
        /// 商家数据包
        /// </summary>
        public string Attach { get; set; }
        /// <summary>
        /// 支付完成时间
        /// </summary>
        public DateTime PayTime { get; set; }
    }
}
