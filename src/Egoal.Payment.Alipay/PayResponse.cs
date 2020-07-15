using Egoal.Extensions;
using System;
using System.Collections.Generic;

namespace Egoal.Payment.Alipay
{
    public class PayResponse : AlipayResponse
    {
        public string trade_no { get; set; }
        public string out_trade_no { get; set; }
        public string buyer_logon_id { get; set; }
        public string settle_amount { get; set; }
        public string pay_currency { get; set; }
        public string pay_amount { get; set; }
        public string settle_trans_rate { get; set; }
        public string trans_pay_rate { get; set; }
        public decimal total_amount { get; set; }
        public string trans_currency { get; set; }
        public string settle_currency { get; set; }
        public string receipt_amount { get; set; }
        public decimal? buyer_pay_amount { get; set; }
        public decimal? point_amount { get; set; }
        public decimal? invoice_amount { get; set; }
        public DateTime gmt_payment { get; set; }
        public List<TradeFundBill> fund_bill_list { get; set; }
        public decimal? card_balance { get; set; }
        public string store_name { get; set; }
        public string buyer_user_id { get; set; }
        public string discount_goods_detail { get; set; }
        public List<VoucherDetail> voucher_detail_list { get; set; }
        public string advance_amount { get; set; }
        public string auth_trade_pay_mode { get; set; }
        public string charge_amount { get; set; }
        public string charge_flags { get; set; }
        public string settlement_id { get; set; }
        public string business_params { get; set; }
        public string buyer_user_type { get; set; }
        public string mdiscount_amount { get; set; }
        public string discount_amount { get; set; }
        public string buyer_user_name { get; set; }

        public NetPayOutput ToNetPayOutput()
        {
            var output = new NetPayOutput();
            output.OpenId = buyer_user_id;
            if (!fund_bill_list.IsNullOrEmpty())
            {
                output.BankType = fund_bill_list[0].bank_code ?? fund_bill_list[0].fund_channel;
            }
            output.TotalFee = total_amount;
            output.FeeType = pay_currency;
            output.TransactionId = trade_no;
            output.ListNo = out_trade_no;
            output.PayTime = gmt_payment;
            output.ErrorMessage = sub_msg ?? msg;
            output.IsPaid = code == "10000" || sub_code?.ToUpper() == "ACQ.TRADE_HAS_SUCCESS";
            output.IsPaying = sub_code?.ToUpper() == "AOP.ACQ.SYSTEM_ERROR" || sub_code?.ToUpper() == "ACQ.SYSTEM_ERROR";

            return output;
        }
    }
}
