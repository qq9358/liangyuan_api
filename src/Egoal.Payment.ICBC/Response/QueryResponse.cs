using Egoal.Extensions;
using System;

namespace Egoal.Payment.IcbcPay.Response
{
    /// <summary>
    /// 二维码查询响应参数
    /// </summary>
    public class QueryResponse : IcbcPayResponse
    {
        /// <summary>
        /// 交易结果标志，0：支付中，1：支付成功，2：支付失败
        /// </summary>
        public string pay_status { get; set; }

        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string cust_id { get; set; }

        /// <summary>
        /// 屏蔽后的银行卡号
        /// </summary>
        public string card_no { get; set; }

        /// <summary>
        /// 订单总金额，单位：分
        /// </summary>
        public string total_amt { get; set; }

        /// <summary>
        /// 积分抵扣金额，单位：分
        /// </summary>
        public string point_amt { get; set; }

        /// <summary>
        /// 电子券抵扣金额，单位：分
        /// </summary>
        public string ecoupon_amt { get; set; }

        /// <summary>
        /// 优惠立减金额（商户部分），单位：分
        /// </summary>
        public string mer_disc_amt { get; set; }

        /// <summary>
        /// 优惠券金额，单位：分
        /// </summary>
        public string coupon_amt { get; set; }

        /// <summary>
        /// 银行补贴金额，单位：分
        /// </summary>
        public string bank_disc_amt { get; set; }

        /// <summary>
        /// 用户实际扣减金额，单位：分
        /// </summary>
        public string payment_amt { get; set; }

        /// <summary>
        /// 商户系统订单号，原样返回
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 行内系统订单号，（特约商户27位，特约部门30位）
        /// </summary>
        public string order_id { get; set; }

        /// <summary>
        /// 支付完成时间，yyyyMMdd格式。如支付中状态，返回订单生成日期
        /// </summary>
        public string pay_time { get; set; }

        /// <summary>
        /// 总优惠金额，其值=优惠立减金额（商户部分）+银行补贴金额，单位：分
        /// </summary>
        public string total_disc_amt { get; set; }


        public QueryPayOutput ToQueryPayOutput()
        {
            QueryPayOutput queryPayOutput = new QueryPayOutput();
            queryPayOutput.OpenId = cust_id;
            queryPayOutput.TradeState = pay_status;
            if (!total_amt.IsNullOrEmpty())
            {
                queryPayOutput.TotalFee = total_amt.To<decimal>() / 100;
            }
            queryPayOutput.TransactionId = order_id;
            queryPayOutput.ListNo = out_trade_no;
            queryPayOutput.PayTime = pay_time.IsNullOrEmpty() ? DateTime.Now : pay_time.ToDateTime("yyyyMMdd");
            queryPayOutput.IsPaid = pay_status == "1";
            queryPayOutput.IsPaying = pay_status == "0";
            queryPayOutput.IsExist = return_code != 5940021 && return_code != 5940022;
            queryPayOutput.IsRefund = return_code == 93008595 || return_code == 5920006;
            queryPayOutput.ErrorMessage = return_msg;

            return queryPayOutput;
        }
    }
}
