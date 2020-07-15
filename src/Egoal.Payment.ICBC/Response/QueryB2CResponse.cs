using Egoal.Extensions;
using System;

namespace Egoal.Payment.IcbcPay.Response
{
    public class QueryB2CResponse : IcbcPayResponse
    {
        public string serial_no { get; set; }
        public string trx_sqnb { get; set; }
        public string shop_code { get; set; }
        public string sh_acct { get; set; }
        public string cust_name { get; set; }
        public string shzone_no { get; set; }
        public string shbr_no { get; set; }
        public string mcert_id { get; set; }
        public string order_no { get; set; }
        public string order_sum { get; set; }
        public string intename_hex { get; set; }
        public string intevers { get; set; }
        public string sdate { get; set; }
        public string stime { get; set; }
        public string trtimesp { get; set; }
        public string tdate { get; set; }
        public string ttime { get; set; }
        public string csum { get; set; }
        public string rjswitch { get; set; }
        public string rjsum { get; set; }
        public string trx_code { get; set; }
        public string status { get; set; }
        public string group_id { get; set; }
        public string xj_amt { get; set; }
        public string dep_flag { get; set; }
        public string depmonth { get; set; }
        public string depfir_amt { get; set; }
        public string depsh_fee { get; set; }
        public string depcu_fee { get; set; }
        public string depfeepf { get; set; }
        public string dep_serno { get; set; }
        public string trx_serno { get; set; }
        public string re_flag { get; set; }
        public string sfee_type { get; set; }
        public string pay_type { get; set; }
        public string pay_mode { get; set; }
        public string ctrx_serno { get; set; }
        public string merno { get; set; }
        public string thbankno { get; set; }
        public string field7 { get; set; }
        public string point_flag { get; set; }
        public string paid_point { get; set; }
        public string rjpaid_point { get; set; }
        public string point2_amt { get; set; }
        public string rjpoint2_amt { get; set; }
        public string p2shop_amt { get; set; }
        public string rjp2shop_amt { get; set; }
        public string th_amt { get; set; }
        public string yh_amt { get; set; }
        public string bt_amt { get; set; }
        public string mbt_amt { get; set; }
        public string dzq_flag { get; set; }
        public string tgflag { get; set; }
        public string o2o_paymode { get; set; }
        public string dis_amt { get; set; }
        public string tot_amt { get; set; }
        public string dzq_amt { get; set; }
        public string rjdzq_amt { get; set; }
        public string used_amt { get; set; }
        public string dzq_id { get; set; }
        public string queryTime { get; set; }

        public QueryPayOutput ToQueryPayOutput()
        {
            QueryPayOutput queryPayOutput = new QueryPayOutput();
            queryPayOutput.TradeState = status;
            if (!order_sum.IsNullOrEmpty())
            {
                queryPayOutput.TotalFee = order_sum.To<decimal>() / 100;
            }
            queryPayOutput.TransactionId = trx_serno;
            queryPayOutput.ListNo = order_no;
            queryPayOutput.PayTime = tdate.IsNullOrEmpty() ? DateTime.Now : $"{tdate} {ttime}".ToDateTime("yyyy-MM-dd HH.mm.ss");
            queryPayOutput.IsPaid = status == "2";
            queryPayOutput.IsPaying = status == "4";
            queryPayOutput.IsExist = return_code != 5940021 && return_code != 5940022;
            queryPayOutput.IsRefund = return_code == 93008595 || return_code == 5920006;
            queryPayOutput.ErrorMessage = return_msg;

            return queryPayOutput;
        }
    }
}
