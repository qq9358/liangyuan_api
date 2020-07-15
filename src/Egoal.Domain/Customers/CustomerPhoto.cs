using Egoal.Domain.Entities;
using System;

namespace Egoal.Customers
{
    public class CustomerPhoto : Entity<Guid>
    {
        public Guid? CustomerId { get; set; }
        public byte[] Photo { get; set; }
        public DateTime? Ctime { get; set; }
        public int? ParkId { get; set; }
        public string ParkName { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
