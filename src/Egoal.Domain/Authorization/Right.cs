using Egoal.Domain.Entities;
using System;

namespace Egoal.Authorization
{
    public class Right : Entity
    {
        public int? Pid { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Version { get; set; }
        public int? MenuTypeId { get; set; }
        public bool? MaxFlag { get; set; }
        public int? VisibleTypeId { get; set; }
        public SystemType? SystemTypeId { get; set; }
        public Guid UniqueCode { get; set; }
    }
}
