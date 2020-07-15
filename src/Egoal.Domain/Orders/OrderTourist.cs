using Egoal.Domain.Entities;

namespace Egoal.Orders
{
    public class OrderTourist : Entity<long>
    {
        public long OrderDetailId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public int? CertType { get; set; }
        public string CertNo { get; set; }

        public virtual OrderDetail OrderDetail { get; set; }
    }
}
