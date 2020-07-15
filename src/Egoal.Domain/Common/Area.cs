using Egoal.Domain.Entities;

namespace Egoal.Common
{
    public class Area : Entity
    {
        public int? KeYuanTypeId { get; set; }
        public int? Pid { get; set; }
        public string Name { get; set; }
        public string SortCode { get; set; }
    }
}
