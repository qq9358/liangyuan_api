using Egoal.Application.Services;
using Egoal.Application.Services.Dto;
using Egoal.Caches;
using Egoal.Common;
using Egoal.Domain.Repositories;
using Egoal.Extensions;
using Egoal.Scenics;
using Egoal.Scenics.Dto;
using Egoal.Stadiums;
using Egoal.Tickets;
using Egoal.TicketTypes.Dto;
using Egoal.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.TicketTypes
{
    public class TicketTypeQueryAppService : ApplicationService, ITicketTypeQueryAppService
    {
        private readonly TicketTypeOptions _ticketTypeOptions;
        private readonly ITicketTypeRepository _ticketTypeRepository;
        private readonly IRepository<TicketTypeDescription> _ticketTypeDescriptionRepository;
        private readonly IRepository<TicketTypeClass> _ticketTypeClassRepository;
        private readonly IRepository<TicketTypeClassDetail> _ticketTypeClassDetailRepository;
        private readonly IRepository<TicketTypeAgeRange> _ticketTypeAgeRangeRepository;
        private readonly ITicketTypeDomainService _ticketTypeDomainService;
        private readonly IRepository<GroundChangCiPlan> _groundChangCiPlanRepository;
        private readonly IRepository<Ground> _groundRepository;
        private readonly IRepository<ChangCi> _changCiRepository;
        private readonly IStadiumDomainService _stadiumDomainService;
        private readonly IScenicDomainService _scenicDomainService;
        private readonly INameCacheService _nameCacheService;

        public TicketTypeQueryAppService(
            IOptions<TicketTypeOptions> ticketTypeOptions,
            ITicketTypeRepository ticketTypeRepository,
            IRepository<TicketTypeDescription> ticketTypeDescriptionRepository,
            IRepository<TicketTypeClass> ticketTypeClassRepository,
            IRepository<TicketTypeClassDetail> ticketTypeClassDetailRepository,
            IRepository<TicketTypeAgeRange> ticketTypeAgeRangeRepository,
            ITicketTypeDomainService ticketTypeDomainService,
            IRepository<GroundChangCiPlan> groundChangCiPlanRepository,
            IRepository<Ground> groundRepository,
            IRepository<ChangCi> changCiRepository,
            IStadiumDomainService stadiumDomainService,
            IScenicDomainService scenicDomainService,
            INameCacheService nameCacheService)
        {
            _ticketTypeOptions = ticketTypeOptions.Value;
            _ticketTypeClassRepository = ticketTypeClassRepository;
            _ticketTypeClassDetailRepository = ticketTypeClassDetailRepository;
            _ticketTypeAgeRangeRepository = ticketTypeAgeRangeRepository;
            _ticketTypeRepository = ticketTypeRepository;
            _ticketTypeDescriptionRepository = ticketTypeDescriptionRepository;
            _ticketTypeDomainService = ticketTypeDomainService;
            _groundChangCiPlanRepository = groundChangCiPlanRepository;
            _groundRepository = groundRepository;
            _changCiRepository = changCiRepository;
            _stadiumDomainService = stadiumDomainService;
            _scenicDomainService = scenicDomainService;
            _nameCacheService = nameCacheService;
        }

        public async Task<List<TicketTypeForSaleListDto>> GetTicketTypesForSaleAsync(GetTicketTypesForSaleInput input)
        {
            if (input.SaleDate.IsNullOrEmpty())
            {
                input.SaleDate = DateTime.Now.ToDateString();
            }

            var ticketTypeDtos = new List<TicketTypeForSaleListDto>();

            var ticketTypes = await _ticketTypeRepository.GetTicketTypesForSaleAsync(input);
            foreach (var ticketType in ticketTypes)
            {
                if (input.GroundId.HasValue)
                {
                    if (!await _ticketTypeDomainService.HasGrantedToGroundAsync(ticketType.Id, input.GroundId.Value))
                    {
                        continue;
                    }
                }
                else
                {
                    if (!await _ticketTypeDomainService.HasSpecifiedCheckGroundAsync(ticketType.Id))
                    {
                        continue;
                    }
                }

                if (input.StaffId.HasValue && !await _ticketTypeDomainService.HasGrantedToStaffAsync(ticketType.Id, input.StaffId.Value))
                {
                    continue;
                }

                if (input.SalePointId.HasValue && !await _ticketTypeDomainService.HasGrantedToSalePointAsync(ticketType.Id, input.SalePointId.Value))
                {
                    continue;
                }

                if (input.ParkId.HasValue && !await _ticketTypeDomainService.HasGrantedToParkAsync(ticketType.Id, input.ParkId.Value))
                {
                    continue;
                }

                if (input.SaleChannel == SaleChannel.Local)
                {
                    if (!await _ticketTypeDomainService.HasGroundSharingAsync(ticketType.Id))
                    {
                        continue;
                    }
                }

                var price = await _ticketTypeDomainService.GetPriceAsync(ticketType, input.SaleDate.To<DateTime>(), input.SaleChannel);
                if (price == null)
                {
                    continue;
                }

                var ticketTypeClassIds = await _ticketTypeClassDetailRepository.GetAll()
                    .Where(t => t.TicketTypeId == ticketType.Id)
                    .Select(t => t.TicketTypeClassId)
                    .ToListAsync();

                var ticketTypeDto = new TicketTypeForSaleListDto();
                ticketTypeDto.Id = ticketType.Id;
                ticketTypeDto.Name = ticketType.GetDisplayName();
                ticketTypeDto.StartTravelDate = ticketType.GetStartTravelDate(input.SaleChannel);
                ticketTypeDto.Price = price.Value;
                ticketTypeDto.AllowRefund = ticketType.AllowRefund;
                ticketTypeDto.RefundLimited = ticketType.IsRefundLimited();
                ticketTypeDto.Classes = ticketTypeClassIds.Select(id => _nameCacheService.GetTicketTypeClassName(id)).ToList();
                ticketTypeDto.AllowInlandTourist = ticketType.AllowInlandTourist();
                ticketTypeDto.AllowOverseaTourist = ticketType.AllowOverseaTourist();
                ticketTypeDto.FirstActiveFlag = ticketType.FirstActiveFlag == true;
                ticketTypeDto.StatGroupId = ticketType.StatGroupId;
                ticketTypeDto.WxShowQrCode = ticketType.WxShowQrCode;
                ticketTypeDto.UsageMethod = ticketType.UsageMethod;
                List<TicketTypeAgeRange> ticketTypeAgeRanges = await GetTicketTypeAgeRangeAsync(ticketType.Id);
                List<Dto.TicketTypeAgeRange> dtoTicketTypeAgeRanges = new List<Dto.TicketTypeAgeRange>();
                foreach(TicketTypeAgeRange ticketTypeAgeRange in ticketTypeAgeRanges)
                {
                    dtoTicketTypeAgeRanges.Add(ticketTypeAgeRange.ToDtoTicketTypeAgeRange());
                }
                ticketTypeDto.TicketTypeAgeRanges = dtoTicketTypeAgeRanges;
                ticketTypeDtos.Add(ticketTypeDto);
            }

            return ticketTypeDtos;
        }

        public async Task<PagedResultDto<TicketTypeDescriptionDto>> GetTicketTypeDescriptionsAsync(GetTicketTypeDescriptionsInput input)
        {
            var query = _ticketTypeDescriptionRepository.GetAll()
                 .WhereIf(input.TicketTypeId.HasValue, t => t.TicketTypeId == input.TicketTypeId);

            var count = await _ticketTypeDescriptionRepository.CountAsync(query);

            query = query.OrderBy(t => t.Id).PageBy(input);

            var descriptions = await _ticketTypeDescriptionRepository.ToListAsync(query);

            int maxLength = 15;
            var items = new List<TicketTypeDescriptionDto>();
            foreach (var description in descriptions)
            {
                var item = new TicketTypeDescriptionDto();
                item.TicketTypeId = description.TicketTypeId;
                item.TicketTypeName = _nameCacheService.GetTicketTypeName(description.TicketTypeId);
                item.BookDescription = description.BookDescription?.Length > maxLength ? description.BookDescription.Substring(0, maxLength) : description.BookDescription;
                item.FeeDescription = description.FeeDescription?.Length > maxLength ? description.FeeDescription.Substring(0, maxLength) : description.FeeDescription;
                item.UsageDescription = description.UsageDescription?.Length > maxLength ? description.UsageDescription.Substring(0, maxLength) : description.UsageDescription;
                item.RefundDescription = description.RefundDescription?.Length > maxLength ? description.RefundDescription.Substring(0, maxLength) : description.RefundDescription;
                item.OtherDescription = description.OtherDescription?.Length > maxLength ? description.OtherDescription.Substring(0, maxLength) : description.OtherDescription;
                items.Add(item);
            }

            return new PagedResultDto<TicketTypeDescriptionDto>(count, items);
        }

        public async Task<TicketTypeDescriptionDto> GetTicketTypeDescriptionAsync(int ticketTypeId)
        {
            var descriptionDto = new TicketTypeDescriptionDto();

            var description = await _ticketTypeDescriptionRepository.FirstOrDefaultAsync(t => t.TicketTypeId == ticketTypeId);
            if (description != null)
            {
                descriptionDto.TicketTypeId = description.TicketTypeId;
                descriptionDto.BookDescription = description.BookDescription;
                descriptionDto.FeeDescription = description.FeeDescription;
                descriptionDto.UsageDescription = description.UsageDescription;
                descriptionDto.RefundDescription = description.RefundDescription;
                descriptionDto.OtherDescription = description.OtherDescription;
            }

            return descriptionDto;
        }

        public async Task<TicketTypeForSaleListDto> GetTicketTypeByCertNoAsync(GetByCertNoInput input)
        {
            var tourist = new TicketSaleBuyer();
            tourist.SetIdCardNo(input.CertNo);

            var ticketTypeId = await _ticketTypeAgeRangeRepository.GetAll()
                .Where(t => t.StartAge <= tourist.Age && t.EndAge >= tourist.Age)
                .Select(t => t.TicketTypeId)
                .FirstOrDefaultAsync();
            if (ticketTypeId == 0)
            {
                throw new UserFriendlyException("年龄段票类未定义");
            }

            var ticketType = await _ticketTypeRepository.GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == ticketTypeId);
            if (ticketType == null)
            {
                throw new UserFriendlyException("票类未定义");
            }

            var price = await _ticketTypeDomainService.GetPriceAsync(ticketType, input.TravelDate, input.SaleChannel);
            if (price == null)
            {
                throw new UserFriendlyException("票价未定义");
            }

            var ticketTypeDto = new TicketTypeForSaleListDto();
            ticketTypeDto.Id = ticketType.Id;
            ticketTypeDto.Name = ticketType.GetDisplayName();
            ticketTypeDto.Price = price.Value;
            ticketTypeDto.AllowRefund = ticketType.AllowRefund;
            ticketTypeDto.RefundLimited = ticketType.IsRefundLimited();

            return ticketTypeDto;
        }

        public async Task<TicketTypeForNetSaleDto> GetTicketTypeForNetSaleAsync(int ticketTypeId, SaleChannel saleChannel)
        {
            var ticketTypeDto = new TicketTypeForNetSaleDto();

            var query = _ticketTypeRepository.GetAllIncluding(t => t.TicketTypeGrounds).Where(t => t.Id == ticketTypeId);
            var ticketType = await _ticketTypeRepository.FirstOrDefaultAsync(query);
            if (ticketType == null)
            {
                throw new UserFriendlyException($"票类编号{ticketTypeId}不存在");
            }

            ticketTypeDto.Id = ticketType.Id;
            ticketTypeDto.Name = ticketType.GetDisplayName();
            ticketTypeDto.StartTravelDate = ticketType.GetStartTravelDate(saleChannel);
            ticketTypeDto.AllowRefund = ticketType.AllowRefund;
            ticketTypeDto.RefundLimited = ticketType.IsRefundLimited();
            ticketTypeDto.MinBuyNum = ticketType.MinBuyNum ?? 0;
            ticketTypeDto.NeedCert = ticketType.NeedCertFlag ?? false;
            ticketTypeDto.NeedContact = _ticketTypeOptions.NeedContact == "1";

            ticketTypeDto.GroundChangCis = await GetTicketTypeChangCiComboboxItemsAsync(ticketType, ticketTypeDto.StartTravelDate);

            var today = DateTime.Now.Date;
            ticketTypeDto.DailyPrices.Add(new
            {
                Date = today.ToDateString(),
                Price = await _ticketTypeDomainService.GetPriceAsync(ticketType, today, saleChannel),
                Disable = today != ticketTypeDto.StartTravelDate
            });

            var endValidDate = Convert.ToDateTime(ticketType.ValidDate);
            var endTravelDate = ticketTypeDto.StartTravelDate.AddDays(30);
            if (endTravelDate > endValidDate)
            {
                endTravelDate = endValidDate;
            }
            var prices = await _ticketTypeDomainService.GetPriceAsync(ticketTypeId, ticketTypeDto.StartTravelDate.ToDateString(), endTravelDate.ToDateString());
            foreach (var price in prices)
            {
                if (price.Date == today) continue;

                ticketTypeDto.DailyPrices.Add(new
                {
                    Date = price.Date.ToDateString(),
                    Price = saleChannel == SaleChannel.Net ? price.NetPrice : price.TicPrice,
                    Disable = false
                });
            }

            return ticketTypeDto;
        }

        public async Task<List<GroundChangCisDto>> GetTicketTypeChangCiComboboxItemsAsync(int ticketTypeId, DateTime date)
        {
            var query = _ticketTypeRepository.GetAllIncluding(t => t.TicketTypeGrounds).Where(t => t.Id == ticketTypeId);
            var ticketType = await _ticketTypeRepository.FirstOrDefaultAsync(query);

            return await GetTicketTypeChangCiComboboxItemsAsync(ticketType, date);
        }

        private async Task<List<GroundChangCisDto>> GetTicketTypeChangCiComboboxItemsAsync(TicketType ticketType, DateTime date)
        {
            var changCis = new List<GroundChangCisDto>();

            foreach (var ticketTypeGround in ticketType.TicketTypeGrounds)
            {
                var ground = await _groundRepository.GetAll().AsNoTracking().Where(g => g.Id == ticketTypeGround.GroundId).FirstOrDefaultAsync();

                if (ground.SeatSaleFlag != true && ground.ChangCiSaleFlag != true)
                {
                    continue;
                }

                var changCiDto = new GroundChangCisDto();
                changCiDto.GroundId = ground.Id;
                changCiDto.GroundName = ground.Name;
                changCiDto.HasGroundSeat = ground.SeatSaleFlag == true;
                changCiDto.HasGroundChangCi = ground.ChangCiSaleFlag == true;

                var week = (int)date.DayOfWeek;
                if (week == 0)
                {
                    week = 7;
                }

                var validChangCis = new List<ChangCi>();
                var groundChangCis = await _groundChangCiPlanRepository.GetAllListAsync(c => c.GroundId == ground.Id && c.Week == week.ToString());
                foreach (var groundChangCi in groundChangCis)
                {
                    var changCi = await _changCiRepository.FirstOrDefaultAsync(groundChangCi.ChangCiId);

                    if (date.Date == DateTime.Now.Date)
                    {
                        if (ground.ChangCiDelaySaleMinutes.HasValue)
                        {
                            var startTime = $"{DateTime.Now.ToDateString()} {changCi.Stime.Substring(0, 5):00}";
                            if (startTime.To<DateTime>().AddMinutes(ground.ChangCiDelaySaleMinutes.Value) < DateTime.Now)
                            {
                                continue;
                            }
                        }

                        var endTime = $"{DateTime.Now.ToDateString()} {changCi.Etime.Substring(0, 5)}:00";
                        if (endTime.To<DateTime>() < DateTime.Now)
                        {
                            continue;
                        }
                    }

                    if (ground.SeatSaleFlag == true && ground.StadiumId.HasValue)
                    {
                        var seatSurplusQuantity = await _stadiumDomainService.GetSeatSurplusQuantityAsync(date, groundChangCi.ChangCiId, ground.StadiumId.Value);
                        if (seatSurplusQuantity <= 0)
                        {
                            continue;
                        }
                    }
                    else if (ground.ChangCiSaleFlag == true)
                    {
                        var surplusQuantity = await _scenicDomainService.GetGroundSeatSurplusQuantityAsync(ground, date, groundChangCi.ChangCiId);
                        if (surplusQuantity <= 0)
                        {
                            continue;
                        }
                    }

                    validChangCis.Add(changCi);
                }

                changCiDto.ChangCis = validChangCis
                    .OrderBy(c => c.Stime)
                    .Select(c => new ComboboxItemDto<int> { Value = c.Id, DisplayText = c.Name })
                    .ToList();

                changCis.Add(changCiDto);
            }

            return changCis;
        }

        public async Task<List<ComboboxItemDto<int>>> GetTicketTypeTypeComboboxItemsAsync()
        {
            return await _ticketTypeRepository.GetTicketTypeTypeComboboxItemsAsync();
        }

        public async Task<List<ComboboxItemDto<int>>> GetTicketTypeComboboxItemsAsync(TicketTypeType? ticketTypeTypeId)
        {
            var query = _ticketTypeRepository.GetAll()
                .WhereIf(ticketTypeTypeId.HasValue, t => t.TicketTypeTypeId == ticketTypeTypeId)
                .OrderBy(t => t.SortCode)
                .Select(t => new ComboboxItemDto<int>
                {
                    Value = t.Id,
                    DisplayText = t.Name
                });

            var items = await _ticketTypeRepository.ToListAsync(query);

            return items;
        }

        public async Task<List<ComboboxItemDto<int>>> GetNetSaleTicketTypeComboboxItemsAsync()
        {
            var query = _ticketTypeRepository.GetAll()
                .Where(t => t.XsTypeId >= 2)
                .OrderBy(t => t.SortCode)
                .Select(t => new ComboboxItemDto<int>
                {
                    Value = t.Id,
                    DisplayText = t.Name
                });

            var items = await _ticketTypeRepository.ToListAsync(query);

            return items;
        }

        public async Task<List<ComboboxItemDto<int>>> GetTicketTypeClassComboboxItemsAsync()
        {
            var query = _ticketTypeClassRepository.GetAll()
                .OrderBy(t => t.SortCode)
                .Select(t => new ComboboxItemDto<int>
                {
                    Value = t.Id,
                    DisplayText = t.Name
                });

            var items = await _ticketTypeClassRepository.ToListAsync(query);

            return items;
        }

        /// <summary>
        /// 查询票类的年龄段
        /// </summary>
        /// <param name="ticketTypeId"></param>
        /// <returns></returns>
        public async Task<List<TicketTypeAgeRange>> GetTicketTypeAgeRangeAsync(int ticketTypeId)
        {
            List<TicketTypeAgeRange> ticketTypeAgeRanges = await _ticketTypeAgeRangeRepository.GetAll().Where(t => t.TicketTypeId == ticketTypeId).ToListAsync();
            return ticketTypeAgeRanges;
        }
    }
}
