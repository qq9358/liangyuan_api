using Egoal.Domain.Repositories;
using Egoal.Domain.Services;
using Egoal.UI;
using System;
using System.Threading.Tasks;

namespace Egoal.Customers
{
    public class CustomerDomainService : DomainService, ICustomerDomainService
    {
        private readonly IRepository<CustomerMemberBind> _customerMemberBindRepository;

        public CustomerDomainService(IRepository<CustomerMemberBind> customerMemberBindRepository)
        {
            _customerMemberBindRepository = customerMemberBindRepository;
        }

        public async Task BindMemberAsync(Guid customerId, Guid memberId)
        {
            bool hasBind = await _customerMemberBindRepository.AnyAsync(cm => cm.MemberId == memberId && cm.CustomerId != customerId);
            if (hasBind)
            {
                throw new UserFriendlyException("已绑定其他客户");
            }

            var bindInfo = await _customerMemberBindRepository.FirstOrDefaultAsync(cm => cm.CustomerId == customerId);
            if (bindInfo == null)
            {
                bindInfo = new CustomerMemberBind();
                bindInfo.CustomerId = customerId;
                bindInfo.MemberId = memberId;

                await _customerMemberBindRepository.InsertAsync(bindInfo);
            }
            else if (bindInfo.MemberId != memberId)
            {
                throw new UserFriendlyException("已绑定其他人");
            }
        }

        public async Task UnBindMemberAsync(Guid customerId, Guid memberId)
        {
            await _customerMemberBindRepository.DeleteAsync(cm => cm.CustomerId == customerId && cm.MemberId == memberId);
        }

        public async Task<Guid?> GetBindingCustomerIdAsync(Guid memberId)
        {
            var bindInfo = await _customerMemberBindRepository.FirstOrDefaultAsync(cm => cm.MemberId == memberId);

            return bindInfo?.CustomerId;
        }

        public async Task<Guid?> GetBindingMemberIdAsync(Guid customerId)
        {
            var bindInfo = await _customerMemberBindRepository.FirstOrDefaultAsync(cm => cm.CustomerId == customerId);

            return bindInfo?.MemberId;
        }
    }
}
