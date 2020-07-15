using Egoal.Domain.Entities;

namespace Egoal.Common
{
    public class ChangCi : Entity
    {
        public string Name { get; set; }
        public string SortCode { get; set; }
        public int? CcTypeId { get; set; }
        public string Stime { get; set; }
        public string Etime { get; set; }
        public int? ChangCiNum { get; set; }
        public int? Minutes { get; set; }
    }
}
