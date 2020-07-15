namespace Egoal.Tickets
{
    public class TicketSaleOptions
    {
        /// <summary>
        /// 证件购票周期(配合“证件可购次数”参数使用,0为不限)
        /// </summary>
        public int CertTicketSaleDaysRange { get; set; }

        /// <summary>
        /// 证件可购次数(周期内可购次数,0为不限)
        /// </summary>
        public int CertTicketSaleNum { get; set; }
    }
}
