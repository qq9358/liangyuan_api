using Egoal.Domain.Entities;

namespace Egoal.Tickets
{
    public class TicketSaleDayStat : Entity<long>
    {
        public int TicketNum { get; set; }
        public int PersonNum { get; set; }
        public decimal TicMoney { get; set; }
        public int TicketTypeId { get; set; }
        public int CashierId { get; set; }
        public int CashPcid { get; set; }
        public string Cdate { get; set; }
        public string Ctp { get; set; }
    }
}
