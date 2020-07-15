using Egoal.Stadiums.Dto;
using Egoal.UI;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.Linq;
using Egoal.Domain.Services;
using Egoal.Domain.Repositories;
using System.Collections.Generic;

namespace Egoal.Stadiums
{
    public class StadiumDomainService : DomainService, IStadiumDomainService
    {
        private readonly StadiumOptions _options;
        private readonly IRepository<Stadium> _stadiumRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly ISeatStatusCacheRepository _seatStatusCacheRepository;

        public StadiumDomainService(
            IOptions<StadiumOptions> options,
            IRepository<Stadium> stadiumRepository,
            ISeatRepository seatRepository,
            ISeatStatusCacheRepository seatStatusCacheRepository)
        {
            _options = options.Value;
            _stadiumRepository = stadiumRepository;
            _seatRepository = seatRepository;
            _seatStatusCacheRepository = seatStatusCacheRepository;
        }

        public async Task<int> GetSeatSurplusQuantityAsync(DateTime date, int changCiId, int stadiumId)
        {
            var stadium = await _stadiumRepository.FirstOrDefaultAsync(stadiumId);

            var lockTime = -_options.SeatLockMinutes;
            var disabledSeatQuantity = await _seatStatusCacheRepository.GetDisabledSeatQuantityAsync(date, changCiId, stadiumId, lockTime);

            return stadium.SeatNum ?? 0 - disabledSeatQuantity;
        }

        public async Task<List<long>> SeatingAsync(SeatingInput input)
        {
            input.LockMinutes = -_options.SeatLockMinutes;

            var seats = await _seatRepository.GetSeatForSaleAsync(input);
            if (seats.Count < input.Quantity)
            {
                throw new UserFriendlyException("座位锁定失败，请稍后重试");
            }

            var seatIds = new List<long>();
            foreach (var seat in seats)
            {
                if (seat.StatusCacheId.HasValue)
                {
                    if (!await _seatStatusCacheRepository.ReSaleAsync(seat.StatusCacheId.Value, input.ListNo, seat.StatusID.Value, seat.LockTime.Value))
                    {
                        throw new UserFriendlyException("座位锁定失败，请稍后重试");
                    }
                }
                else
                {
                    SeatStatusCache seatStatusCache = new SeatStatusCache();
                    seatStatusCache.SeatId = seat.Id;
                    seatStatusCache.ChangCiId = input.ChangCiId;
                    seatStatusCache.Sdate = input.Date;
                    seatStatusCache.StatusId = SeatStatus.已售;
                    seatStatusCache.LockTime = DateTime.Now;
                    seatStatusCache.ListNo = input.ListNo;
                    await _seatStatusCacheRepository.SaleAsync(seatStatusCache);
                }

                seatIds.Add(seat.Id);
            }

            return seatIds;
        }

        public async Task CancelSeatingAsync(string listNo, long? ticketId, int quantity)
        {
            if (ticketId.HasValue)
            {
                var query = _seatStatusCacheRepository.GetAll().Where(s => s.ListNo == listNo && s.TicketId == ticketId).OrderBy(s => s.SeatId).Take(quantity);
                var seats = await _seatStatusCacheRepository.ToListAsync(query);
                foreach (var seat in seats)
                {
                    await _seatStatusCacheRepository.DeleteAsync(seat);
                }
            }
            else
            {
                await _seatStatusCacheRepository.CancelAsync(listNo);
            }
        }
    }
}
