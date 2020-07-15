using Egoal.Domain.Entities;

namespace Egoal.Staffs
{
    public class ExplainerTimeslot : Entity
    {
        public string Name { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
    }
}
