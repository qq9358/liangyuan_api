using Egoal.Application.Services;
using Egoal.Caches;
using Egoal.Domain.Repositories;
using Egoal.TicketTypes.Dto;
using Egoal.UI;
using System.Threading.Tasks;

namespace Egoal.TicketTypes
{
    public class TicketTypeAppService : ApplicationService, ITicketTypeAppService
    {
        private readonly IRepository<TicketTypeDescription> _ticketTypeDescriptionRepository;
        private readonly INameCacheService _nameCacheService;

        public TicketTypeAppService(
            IRepository<TicketTypeDescription> ticketTypeDescriptionRepository,
            INameCacheService nameCacheService)
        {
            _ticketTypeDescriptionRepository = ticketTypeDescriptionRepository;
            _nameCacheService = nameCacheService;
        }

        public async Task CreateDescriptionAsync(TicketTypeDescriptionDto input)
        {
            var count = await _ticketTypeDescriptionRepository.CountAsync(t => t.TicketTypeId == input.TicketTypeId);
            if (count > 0)
            {
                throw new UserFriendlyException($"{_nameCacheService.GetTicketTypeName(input.TicketTypeId)}票类说明已存在");
            }

            var description = new TicketTypeDescription();
            description.TicketTypeId = input.TicketTypeId;
            description.BookDescription = input.BookDescription;
            description.FeeDescription = input.FeeDescription;
            description.UsageDescription = input.UsageDescription;
            description.RefundDescription = input.RefundDescription;
            description.OtherDescription = input.OtherDescription;

            await _ticketTypeDescriptionRepository.InsertAsync(description);
        }

        public async Task UpdateDescriptionAsync(TicketTypeDescriptionDto input)
        {
            var description = await _ticketTypeDescriptionRepository.FirstOrDefaultAsync(t => t.TicketTypeId == input.TicketTypeId);
            if (description == null)
            {
                throw new UserFriendlyException($"{_nameCacheService.GetTicketTypeName(input.TicketTypeId)}票类说明不存在");
            }

            description.BookDescription = input.BookDescription;
            description.FeeDescription = input.FeeDescription;
            description.UsageDescription = input.UsageDescription;
            description.RefundDescription = input.RefundDescription;
            description.OtherDescription = input.OtherDescription;
        }

        public async Task DeleteDescriptionAsync(int ticketTypeId)
        {
            var description = await _ticketTypeDescriptionRepository.FirstOrDefaultAsync(t => t.TicketTypeId == ticketTypeId);

            await _ticketTypeDescriptionRepository.DeleteAsync(description);
        }
    }
}
