using Egoal.Domain.Entities;
using System;

namespace Egoal.Members
{
    public class UserWechat : Entity
    {
        public Guid UserId { get; set; }
        public string Nickname { get; set; }
        public string HeadImageUrl { get; set; }
        public string UnionId { get; set; }
        public string OffiaccountOpenId { get; set; }
        public string MiniProgramOpenId { get; set; }

        public virtual Member Member { get; set; }
    }
}
