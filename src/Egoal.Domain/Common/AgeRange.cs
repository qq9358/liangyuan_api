using Egoal.Domain.Entities;

namespace Egoal.Common
{
    public class AgeRange : Entity
    {
        public string Name { get; set; }
        public int StartAge { get; set; }
        public int EndAge { get; set; }
    }
}
