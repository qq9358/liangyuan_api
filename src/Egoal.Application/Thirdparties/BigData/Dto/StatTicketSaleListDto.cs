using Egoal.Trades;
using System.Data;

namespace Egoal.Thirdparties.BigData.Dto
{
    public class StatTicketSaleListDto
    {
        public string time_type { get; set; }
        public string trade_source { get; set; }
        public string product_name { get; set; }
        public string price { get; set; }
        public string quantity { get; set; }
        public string total_money { get; set; }

        public static StatTicketSaleListDto FromDataRow(DataRow row)
        {
            var ticketSale = new StatTicketSaleListDto();
            ticketSale.time_type = row["StatType"].ToString();
            if (int.TryParse(row["TradeSource"].ToString(), out int tradeSource))
            {
                ticketSale.trade_source = ((TradeSource)tradeSource).ToString();
            }
            ticketSale.product_name = row["TicketTypeName"].ToString();
            ticketSale.price = row["ReaPrice"].ToString();
            ticketSale.quantity = row["PersonNum"].ToString();
            ticketSale.total_money = row["ReaMoney"].ToString();

            return ticketSale;
        }
    }
}
