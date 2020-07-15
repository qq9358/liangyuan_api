using Egoal.Domain.Entities;

namespace Egoal.Orders
{
    public class OrderAgeRange : Entity<long>
    {
        public string ListNo { get; set; }
        public int AgeRangeId { get; set; }
        public int PersonNum { get; set; }

        public virtual Order Order { get; set; }
    }
}
