using Egoal.Application.Services;
using Egoal.Application.Services.Dto;
using Egoal.Caches;
using Egoal.Domain.Repositories;
using Egoal.Dto;
using Egoal.Notifications;
using Egoal.Runtime.Session;
using Egoal.Staffs.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egoal.Staffs
{
    public class ExplainerAppService : ApplicationService, IExplainerAppService
    {
        private readonly ISession _session;
        private readonly INameCacheService _nameCacheService;
        private readonly IExplainerTimeslotSchedulingRepository _explainerTimeslotSchedulingRepository;
        private readonly IRepository<ExplainerTimeslot> _explainerTimeslotRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IExplainerDomainService _explainerDomainService;
        private readonly IRealTimeNotifier _realTimeNotifier;

        public ExplainerAppService(
            ISession session,
            INameCacheService nameCacheService,
            IExplainerTimeslotSchedulingRepository explainerTimeslotSchedulingRepository,
            IRepository<ExplainerTimeslot> explainerTimeslotRepository,
            IStaffRepository staffRepository,
            IExplainerDomainService explainerDomainService,
            IRealTimeNotifier realTimeNotifier)
        {
            _session = session;
            _nameCacheService = nameCacheService;
            _explainerTimeslotSchedulingRepository = explainerTimeslotSchedulingRepository;
            _explainerTimeslotRepository = explainerTimeslotRepository;
            _staffRepository = staffRepository;
            _explainerDomainService = explainerDomainService;
            _realTimeNotifier = realTimeNotifier;
        }

        public async Task<List<VantPickerItem<int>>> GetSchedulingsAsync(GetSchedulingInput input)
        {
            var schedulings = await _explainerTimeslotSchedulingRepository.GetSchedulingsAsync(input.Date, DateTime.Now.ToString("HH:mm"));
            var query = from scheduling in schedulings
                        where scheduling.PublicQuantity > 0
                        select new VantPickerItem<int>
                        {
                            Value = scheduling.TimeslotId,
                            Text = $"{ _nameCacheService.GetExplainerTimeslotName(scheduling.TimeslotId)}{(scheduling.PublicQuantity <= scheduling.PublicBookedQuantity ? "(己满)" : "")}",
                            Disabled = scheduling.PublicQuantity <= scheduling.PublicBookedQuantity
                        };

            return new List<VantPickerItem<int>> { new VantPickerItem<int> { Text = "讲解场次", Children = query.ToList() } };
        }

        public async Task<List<NameValue>> GetReservedSchedulingComboxItemsAsync(GetSchedulingInput input)
        {
            var schedulings = await _explainerTimeslotSchedulingRepository.GetSchedulingsAsync(input.Date, DateTime.Now.ToString("HH:mm"));

            var items = schedulings
                .Where(s => s.ReservedQuantity > 0 && s.ReservedQuantity > s.ReservedBookedQuantity)
                .Select(s => new NameValue
                {
                    Name = _nameCacheService.GetExplainerTimeslotName(s.TimeslotId),
                    Value = s.TimeslotId.ToString()
                })
                .ToList();

            return items;
        }

        public async Task<List<ComboboxItemDto>> GetExplainerComboboxItemsAsync()
        {
            return await _staffRepository.GetExplainerComboboxItemsAsync();
        }

        public async Task<List<ComboboxItemDto>> GetExplainTimeslotComboboxItemsAsync()
        {
            var query = _explainerTimeslotRepository.GetAll()
                .OrderBy(t => t.BeginTime)
                .Select(t => new ComboboxItemDto
                {
                    DisplayText = t.Name,
                    Value = t.Id.ToString()
                });

            return await _explainerTimeslotRepository.ToListAsync(query);
        }

        public async Task BeginExplainAsync(ExplainInput input)
        {
            await _explainerDomainService.BeginExplainAsync(input.ListNo, _session.StaffId.Value, input.TimeslotId);

            await _realTimeNotifier.NoticeExplainerBeginExplainAsync(input);
        }

        public async Task CompleteExplainAsync(ExplainInput input)
        {
            await _explainerDomainService.CompleteExplainAsync(input.ListNo, _session.StaffId.Value);

            await _realTimeNotifier.NoticeExplainerCompleteExplainAsync(input);
        }
    }
}
