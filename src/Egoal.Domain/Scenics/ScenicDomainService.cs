using Egoal.Domain.Services;
using System;
using System.Threading.Tasks;

namespace Egoal.Scenics
{
    public class ScenicDomainService : DomainService, IScenicDomainService
    {
        private readonly IGroundDateChangCiSaleNumRepository _groundDateChangCiSaleNumRepository;

        public ScenicDomainService(
            IGroundDateChangCiSaleNumRepository groundDateChangCiSaleNumRepository)
        {
            _groundDateChangCiSaleNumRepository = groundDateChangCiSaleNumRepository;
        }

        public async Task<int> GetGroundSeatSurplusQuantityAsync(Ground ground, DateTime date, int changCiId)
        {
            var saleQuantity = await _groundDateChangCiSaleNumRepository.GetSaleQuantityAsync(ground.Id, date, changCiId);

            return ground.SeatNum ?? 0 - saleQuantity;
        }
    }
}
