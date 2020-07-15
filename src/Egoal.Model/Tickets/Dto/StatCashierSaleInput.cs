using System;

namespace Egoal.Tickets.Dto
{
    public class StatCashierSaleInput
    {
        public DateTime StartCTime { get; set; }
        public DateTime EndCTime { get; set; }
        public int? CashierId { get; set; }
        public int? SalePointId { get; set; }
    }
}
