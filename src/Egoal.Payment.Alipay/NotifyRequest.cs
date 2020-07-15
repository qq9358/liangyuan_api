using Egoal.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Egoal.Payment.Alipay
{
    public class NotifyRequest
    {
        public DateTime notify_time { get; set; }
        public string notify_type { get; set; }
        public string notify_id { get; set; }
        public string sign_type { get; set; }
        public string sign { get; set; }
        public string trade_no { get; set; }
        public string app_id { get; set; }
        public string out_trade_no { get; set; }
        public string out_biz_no { get; set; }
        public string buyer_id { get; set; }
        public string buyer_logon_id { get; set; }
        public string seller_id { get; set; }
        public string seller_email { get; set; }
        public string trade_status { get; set; }
        public decimal? total_amount { get; set; }
        public decimal? receipt_amount { get; set; }
        public decimal? invoice_amount { get; set; }
        public decimal? buyer_pay_amount { get; set; }
        public decimal? point_amount { get; set; }
        public decimal? refund_fee { get; set; }
        public decimal? send_back_fee { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public DateTime? gmt_create { get; set; }
        public DateTime? gmt_payment { get; set; }
        public DateTime? gmt_refund { get; set; }
        public DateTime? gmt_close { get; set; }
        public string fund_bill_list { get; set; }

        public NotifyInput ToNotifyInput()
        {
            var input = new NotifyInput();
            input.PaySuccess = trade_status.Equals("TRADE_SUCCESS", StringComparison.OrdinalIgnoreCase) || trade_status.Equals("TRADE_FINISHED", StringComparison.OrdinalIgnoreCase);
            input.AppId = app_id;
            input.MerchantNo = seller_id;
            input.OpenId = buyer_id;
            if (!fund_bill_list.IsNullOrEmpty())
            {
                var channels = fund_bill_list.JsonToObject<List<FundBill>>();
                input.BankType = channels.Select(c => c.fundChannel).FirstOrDefault();
            }
            input.TotalFee = total_amount ?? 0;
            input.TransactionId = trade_no;
            input.ListNo = out_trade_no;
            input.Attach = body;
            input.PayTime = gmt_payment ?? notify_time;

            return input;
        }

        private class FundBill
        {
            public decimal amount { get; set; }
            public string fundChannel { get; set; }
        }
    }
}
