using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Egoal.Customers
{
    public interface ICustomerDomainService
    {
        Task BindMemberAsync(Guid customerId, Guid memberId);
        Task UnBindMemberAsync(Guid customerId, Guid memberId);
        Task<Guid?> GetBindingCustomerIdAsync(Guid memberId);
        Task<Guid?> GetBindingMemberIdAsync(Guid customerId);
    }
}
