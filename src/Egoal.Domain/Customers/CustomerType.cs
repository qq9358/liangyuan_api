using Egoal.Domain.Entities;

namespace Egoal.Customers
{
    public class CustomerType : Entity
    {
        public string Name { get; set; }
        public string SortCode { get; set; }
    }
}
