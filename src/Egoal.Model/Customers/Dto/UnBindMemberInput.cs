using System;

namespace Egoal.Customers.Dto
{
    public class UnBindMemberInput
    {
        public Guid CustomerId { get; set; }
        public Guid MemberId { get; set; }
    }
}
