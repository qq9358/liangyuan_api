using Egoal.Domain.Entities;

namespace Egoal.Settings
{
    public class Setting : Entity<string>
    {
        public string Value { get; set; }
        public string Caption { get; set; }
        public bool EditFlag { get; set; } = true;
        public bool VisibleFlag { get; set; } = true;
    }
}
