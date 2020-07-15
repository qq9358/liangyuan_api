using Egoal.Domain.Entities;
using System;

namespace Egoal.Members
{
    public class MemberGuider : Entity
    {
        public Guid MemberId { get; set; }
        public Guid GuiderId { get; set; }
    }
}
