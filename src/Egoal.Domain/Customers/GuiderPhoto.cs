using Egoal.Domain.Entities;
using System;

namespace Egoal.Customers
{
    public class GuiderPhoto : Entity<Guid>
    {
        public GuiderPhoto()
        {
            Id = Guid.NewGuid();
        }

        public Guid? GuiderId { get; set; }
        public byte[] Photo { get; set; }
        public DateTime? Ctime { get; set; } = DateTime.Now;
        public int? ParkId { get; set; }
        public string ParkName { get; set; }
    }
}
