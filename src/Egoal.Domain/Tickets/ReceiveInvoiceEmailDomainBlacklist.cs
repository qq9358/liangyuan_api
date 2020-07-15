using Egoal.Domain.Entities;

namespace Egoal.Tickets
{
    public class ReceiveInvoiceEmailDomainBlacklist : Entity<long>
    {
        public string EmailDomain { get; set; }
        public string SortCode { get; set; }
    }
}
