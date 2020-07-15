using Egoal.Domain.Entities;

namespace Egoal.Common
{
    public class KeYuanType : Entity
    {
        public string Name { get; set; }
        public string SortCode { get; set; }
        public int? TqMinutes { get; set; }
    }
}
