using Egoal.Domain.Entities;

namespace Egoal.Orders
{
    public class OrderGroundChangCi : Entity<long>
    {
        public long OrderDetailId { get; set; }
        public int GroundId { get; set; }
        public int ChangCiId { get; set; }
        public int Quantity { get; set; }

        public virtual OrderDetail OrderDetail { get; set; }
    }
}
