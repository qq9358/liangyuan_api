using Egoal.Domain.Entities;

namespace Egoal.Scenics
{
    public class GroundChangCiPlan : Entity
    {
        public string Week { get; set; }
        public int ChangCiId { get; set; }
        public int? GroundId { get; set; }
    }
}
