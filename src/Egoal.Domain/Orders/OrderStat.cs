using Egoal.Domain.Entities;

namespace Egoal.Orders
{
    public class OrderStat : Entity
    {
        public string Cdate { get; set; }
        public int OrderNum { get; set; }
        public int? OrderPlanType { get; set; }
    }
}
