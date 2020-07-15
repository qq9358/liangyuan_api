using Egoal.Extensions;
using System;
using System.Collections.Generic;

namespace Egoal.Payment.Alipay
{
    public class QueryResponse : AlipayResponse
    {
        public string trade_no { get; set; }
        public string out_trade_no { get; set; }
        public string buyer_logon_id { get; set; }
        public string trade_status { get; set; }
        public decimal total_amount { get; set; }
        public string trans_currency { get; set; }
        public string settle_currency { get; set; }
        public decimal? settle_amount { get; set; }
        public string pay_currency { get; set; }
        public decimal? pay_amount { get; set; }
        public string settle_trans_rate { get; set; }
        public string trans_pay_rate { get; set; }
        public decimal? buyer_pay_amount { get; set; }
        public decimal? point_amount { get; set; }
        public decimal? invoice_amount { get; set; }
        public DateTime? send_pay_date { get; set; }
        public decimal? receipt_amount { get; set; }
        public string store_id { get; set; }
        public string terminal_id { get; set; }
        public List<TradeFundBill> fund_bill_list { get; set; }
        public string store_name { get; set; }
        public string buyer_user_id { get; set; }
        public decimal? charge_amount { get; set; }
        public string charge_flags { get; set; }
        public string settlement_id { get; set; }
        public string auth_trade_pay_mode { get; set; }
        public string buyer_user_type { get; set; }
        public decimal? mdiscount_amount { get; set; }
        public decimal? discount_amount { get; set; }
        public string buyer_user_name { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string alipay_sub_merchant_id { get; set; }
        public string ext_infos { get; set; }

        public QueryPayOutput ToQueryPayOutput()
        {
            var output = new QueryPayOutput();
            output.DeviceInfo = terminal_id;
            output.OpenId = buyer_user_id;
            output.TradeState = trade_status;
            output.TotalFee = total_amount;
            if (!fund_bill_list.IsNullOrEmpty())
            {
                output.BankType = fund_bill_list[0].bank_code ?? fund_bill_list[0].fund_channel;
            }
            output.FeeType = pay_currency;
            output.TransactionId = trade_no;
            output.ListNo = out_trade_no;
            output.Attach = body;
            output.PayTime = send_pay_date ?? DateTime.Now;
            output.IsPaid = trade_status?.ToUpper() == "TRADE_SUCCESS" || trade_status?.ToUpper() == "TRADE_FINISHED";
            output.IsPaying = trade_status?.ToUpper() == "WAIT_BUYER_PAY" || sub_code?.ToUpper() == "ACQ.SYSTEM_ERROR";
            output.IsExist = sub_code?.ToUpper() != "ACQ.TRADE_NOT_EXIST";
            output.ErrorMessage = sub_msg ?? msg;

            return output;
        }
    }
}
