using Egoal.Domain.Entities;
using System;

namespace Egoal.Members
{
    public class MemberPhoto : Entity<Guid>
    {
        public Guid? MemberId { get; set; }
        public byte[] Photo { get; set; }
        public DateTime? Ctime { get; set; }
        public int? ParkId { get; set; }
        public string ParkName { get; set; }

        public virtual Member Member { get; set; }
    }
}
