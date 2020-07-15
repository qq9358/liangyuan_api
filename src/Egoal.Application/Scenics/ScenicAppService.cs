using Egoal.Application.Services;
using Egoal.Application.Services.Dto;
using Egoal.AutoMapper;
using Egoal.Caches;
using Egoal.Domain.Repositories;
using Egoal.Domain.Uow;
using Egoal.Extensions;
using Egoal.Scenics.Dto;
using Egoal.Stadiums;
using Egoal.Stadiums.Dto;
using Egoal.TicketTypes;
using Egoal.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Scenics
{
    public class ScenicAppService : ApplicationService, IScenicAppService
    {
        private readonly ScenicOptions _scenicOptions;
        private readonly IRepository<Park> _parkRepository;
        private readonly IRepository<Ground> _groundRepository;
        private readonly IRepository<GateGroup> _gateGroupRepository;
        private readonly IRepository<SalePoint> _salePointRepository;
        private readonly IGroundDateChangCiSaleNumRepository _groundDateChangCiSaleNumRepository;
        private readonly IRepository<GroundRemoteBookRecord, long> _groundRemoteBookRecordRepository;
        private readonly IRepository<Scenic> _scenicRepository;
        private readonly IStadiumDomainService _stadiumDomainService;
        private readonly ITicketTypeQueryAppService _ticketTypeQueryAppService;
        private readonly INameCacheService _nameCacheService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ScenicAppService(
            IOptions<ScenicOptions> scenicOptions,
            IRepository<Park> parkRepository,
            IRepository<Ground> groundRepository,
            IRepository<GateGroup> gateGroupRepository,
            IRepository<SalePoint> salePointRepository,
            IGroundDateChangCiSaleNumRepository groundDateChangCiSaleNumRepository,
            IRepository<GroundRemoteBookRecord, long> groundRemoteBookRecordRepository,
            IRepository<Scenic> scenicRepository,
            IStadiumDomainService stadiumDomainService,
            ITicketTypeQueryAppService ticketTypeQueryAppService,
            INameCacheService nameCacheService,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _scenicOptions = scenicOptions.Value;
            _parkRepository = parkRepository;
            _groundRepository = groundRepository;
            _gateGroupRepository = gateGroupRepository;
            _salePointRepository = salePointRepository;
            _groundDateChangCiSaleNumRepository = groundDateChangCiSaleNumRepository;
            _groundRemoteBookRecordRepository = groundRemoteBookRecordRepository;
            _scenicRepository = scenicRepository;
            _stadiumDomainService = stadiumDomainService;
            _ticketTypeQueryAppService = ticketTypeQueryAppService;
            _nameCacheService = nameCacheService;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<ScenicDto> GetScenicAsync(string language)
        {
            var scenicDto = new ScenicDto();

            var scenic = await _scenicRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(s => s.Language == language);
            if (scenic != null)
            {
                scenic.MapTo(scenicDto);

                if (!scenic.Photos.IsNullOrEmpty())
                {
                    var photos = scenic.Photos.JsonToObject<List<PhotoDto>>();
                    scenicDto.PhotoList.AddRange(photos);
                }
            }

            return scenicDto;
        }

        public async Task EditScenicAsync(ScenicDto input)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var scenic = await _scenicRepository.FirstOrDefaultAsync(s => s.Language == input.Language);
                if (scenic == null)
                {
                    scenic = new Scenic();
                }

                input.MapTo(scenic);
                scenic.OpenTime = input.OpenTime ?? string.Empty;
                scenic.CloseTime = input.CloseTime ?? string.Empty;
                scenic.Photos = input.PhotoList.ToJson();

                await _scenicRepository.InsertOrUpdateAsync(scenic);

                await uow.CompleteAsync();
            }

            _scenicOptions.ScenicName = input.ScenicName;
            _scenicOptions.ParkOpenTime = input.OpenTime;
            _scenicOptions.ParkCloseTime = input.CloseTime;
        }

        public async Task<List<BookGroundChangCiOutput>> BookGroundChangCiAsync(BookGroundChangCiInput input)
        {
            var outputs = new List<BookGroundChangCiOutput>();

            var groundChangCis = await _ticketTypeQueryAppService.GetTicketTypeChangCiComboboxItemsAsync(input.TicketTypeId, input.Date.To<DateTime>());
            if (groundChangCis.IsNullOrEmpty())
            {
                return outputs;
            }

            foreach (var groundChangCi in groundChangCis)
            {
                if (groundChangCi.ChangCis.IsNullOrEmpty())
                {
                    throw new UserFriendlyException($"{groundChangCi.GroundName}座位数量不足");
                }

                bool bookChangCiSuccess = false;
                foreach (var changCi in groundChangCi.ChangCis)
                {
                    try
                    {
                        var output = await BookGroundChangCiAsync(groundChangCi.GroundId, input.Date, changCi.Value, input.Quantity, input.ListNo, input.IsRemote);
                        outputs.Add(output);
                        bookChangCiSuccess = true;

                        break;
                    }
                    catch (UserFriendlyException)
                    {
                        if (groundChangCi.HasGroundSeat)
                        {
                            throw;
                        }

                        continue;
                    }
                }
                if (!bookChangCiSuccess)
                {
                    throw new UserFriendlyException($"{groundChangCi.GroundName}座位数量不足");
                }
            }

            return outputs;
        }

        public async Task<BookGroundChangCiOutput> BookGroundChangCiAsync(int groundId, string date, int changCiId, int quantity, string listNo, bool isRemote = false)
        {
            var output = new BookGroundChangCiOutput();

            var ground = await _groundRepository.GetAll().AsNoTracking().Where(g => g.Id == groundId).FirstOrDefaultAsync();
            if (ground.SeatSaleFlag == true)
            {
                SeatingInput seatingInput = new SeatingInput();
                seatingInput.ListNo = listNo;
                seatingInput.StadiumId = ground.StadiumId.Value;
                seatingInput.Date = date;
                seatingInput.ChangCiId = changCiId;
                seatingInput.Quantity = quantity;

                var seatIds = await _stadiumDomainService.SeatingAsync(seatingInput);
                output.Seats = seatIds.Select(s => new NameValue<long> { Value = s, Name = _nameCacheService.GetSeatName(s) }).ToList();

                output.HasGroundSeat = true;
            }
            else if (ground.ChangCiSaleFlag == true)
            {
                if (quantity > ground.SeatNum)
                {
                    throw new UserFriendlyException($"{ground.Name}剩余座位数量不足");
                }

                GroundDateChangCiSaleNum groundDateChangCiSaleNum = new GroundDateChangCiSaleNum();
                groundDateChangCiSaleNum.GroundId = groundId;
                groundDateChangCiSaleNum.Date = date;
                groundDateChangCiSaleNum.ChangCiId = changCiId;
                groundDateChangCiSaleNum.SaleNum = quantity;

                if (!await _groundDateChangCiSaleNumRepository.SaleAsync(groundDateChangCiSaleNum, ground.SeatNum ?? 0))
                {
                    throw new UserFriendlyException($"{ground.Name}剩余座位数量不足");
                }

                if (isRemote)
                {
                    var record = new GroundRemoteBookRecord();
                    record.ListNo = listNo;
                    record.Date = date;
                    record.GroundId = groundId;
                    record.ChangCiId = changCiId;
                    record.Quantity = quantity;

                    await _groundRemoteBookRecordRepository.InsertAsync(record);
                }

                output.HasGroundChangCi = true;
            }

            output.GroundId = groundId;
            output.GroundName = _nameCacheService.GetGroundName(groundId);
            output.ChangCiId = changCiId;
            output.ChangCiName = _nameCacheService.GetChangCiName(changCiId);

            return output;
        }

        public async Task CancelGroundChangCiAsync(CancelGroundChangCiInput input)
        {
            if (input.IsRemote)
            {
                await CancelGroundChangCiRemoteAsync(input);
                return;
            }

            if (input.HasGroundSeat)
            {
                await _stadiumDomainService.CancelSeatingAsync(input.ListNo, input.TicketId, input.Quantity);
            }
            else if (input.HasGroundChangCi)
            {
                var groundDateChangCiSaleNum = await _groundDateChangCiSaleNumRepository.FirstOrDefaultAsync(g =>
                g.GroundId == input.GroundId &&
                g.Date == input.Date &&
                g.ChangCiId == input.ChangCiId);
                groundDateChangCiSaleNum.SaleNum -= input.Quantity;
            }
        }

        private async Task CancelGroundChangCiRemoteAsync(CancelGroundChangCiInput input)
        {
            await _stadiumDomainService.CancelSeatingAsync(input.ListNo, input.TicketId, input.Quantity);

            var records = await _groundRemoteBookRecordRepository.GetAllListAsync(r => r.ListNo == input.ListNo);
            foreach (var record in records)
            {
                if (record.IsCanceled) continue;

                var groundDateChangCiSaleNum = await _groundDateChangCiSaleNumRepository.FirstOrDefaultAsync(g =>
                g.GroundId == record.GroundId &&
                g.Date == record.Date &&
                g.ChangCiId == record.ChangCiId);
                groundDateChangCiSaleNum.SaleNum -= record.Quantity;

                record.IsCanceled = true;
            }
        }

        public ScenicOptions GetScenicOptions()
        {
            return _scenicOptions;
        }

        public async Task<List<ComboboxItemDto<int>>> GetParkComboboxItemsAsync()
        {
            var query = _parkRepository.GetAll()
                .OrderBy(p => p.SortCode)
                .Select(p => new ComboboxItemDto<int>
                {
                    Value = p.Id,
                    DisplayText = p.Name
                });

            var parks = await _parkRepository.ToListAsync(query);

            var defaultParks = typeof(DefaultPark).ToComboboxItems();

            return parks.Union(defaultParks, new ComboboxItemDtoComparer<int>()).ToList();
        }

        public async Task<List<ComboboxItemDto<int>>> GetGroundComboboxItemsAsync()
        {
            var query = _groundRepository.GetAll()
                .OrderBy(g => g.SortCode)
                .Select(g => new ComboboxItemDto<int>
                {
                    Value = g.Id,
                    DisplayText = g.Name
                });

            return await _groundRepository.ToListAsync(query);
        }

        public async Task<List<ComboboxItemDto<int>>> GetGateGroupComboboxItemsAsync(int? groundId)
        {
            var query = _gateGroupRepository.GetAll()
                .WhereIf(groundId.HasValue, g => g.GroundId == groundId)
                .OrderBy(g => g.SortCode)
                .Select(g => new ComboboxItemDto<int>
                {
                    Value = g.Id,
                    DisplayText = g.Name
                });

            return await _gateGroupRepository.ToListAsync(query);
        }

        public async Task<List<ComboboxItemDto<int>>> GetSalePointComboboxItemsAsync(int? parkId)
        {
            var query = _salePointRepository.GetAll()
                .Where(s => s.SalePointType == SalePointType.售票点)
                .WhereIf(parkId.HasValue, s => s.ParkId == parkId)
                .OrderBy(g => g.SortCode)
                .Select(g => new ComboboxItemDto<int>
                {
                    Value = g.Id,
                    DisplayText = g.Name
                });

            var salePoints = await _salePointRepository.ToListAsync(query);

            var defaultSalePoints = typeof(DefaultSalePoint).ToComboboxItems();

            return salePoints.Union(defaultSalePoints, new ComboboxItemDtoComparer<int>()).ToList();
        }
    }
}
