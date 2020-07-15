using Egoal.Domain.Entities;

namespace Egoal.Payment
{
    public class PayType : Entity
    {
        public string Name { get; set; }
        public string SortCode { get; set; }
        public bool? PayFlag { get; set; }
        public bool? CzkFlag { get; set; }
        public string Supplier { get; set; }
        public string ServicePhone { get; set; }
    }
}
