using Egoal.Domain.Entities;

namespace Egoal.Settings
{
    public class Constant : Entity<string>
    {
        public string Value { get; set; }
        public string Caption { get; set; }
    }
}
