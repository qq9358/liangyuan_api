using Egoal.Domain.Entities;
using System;

namespace Egoal.Customers
{
    public class CustomerMemberBind : Entity
    {
        public Guid CustomerId { get; set; }
        public Guid MemberId { get; set; }
    }
}
