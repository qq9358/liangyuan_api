using Egoal.Domain.Repositories;
using Egoal.Domain.Services;
using Egoal.Events.Bus;
using Egoal.Orders;
using Egoal.UI;
using System;
using System.Threading.Tasks;

namespace Egoal.Staffs
{
    public class ExplainerDomainService : DomainService, IExplainerDomainService
    {
        private readonly IEventBus _eventBus;
        private readonly IExplainerTimeslotSchedulingRepository _explainerTimeslotSchedulingRepository;
        private readonly IRepository<ExplainerWorkRecord> _explainerWorkRecordRepository;

        public ExplainerDomainService(
            IEventBus eventBus,
            IExplainerTimeslotSchedulingRepository explainerTimeslotSchedulingRepository,
            IRepository<ExplainerWorkRecord> explainerWorkRecordRepository)
        {
            _eventBus = eventBus;
            _explainerTimeslotSchedulingRepository = explainerTimeslotSchedulingRepository;
            _explainerWorkRecordRepository = explainerWorkRecordRepository;
        }

        public async Task BookTimeslotAsync(OrderType orderType, string date, int? timeslotId)
        {
            if (!timeslotId.HasValue)
            {
                return;
            }

            if (orderType == OrderType.微信订票)
            {
                await BookPublicTimeslotAsync(date, timeslotId.Value);
            }
            else if (orderType == OrderType.电话订票)
            {
                await BookReservedTimeslotAsync(date, timeslotId.Value);
            }
        }

        public async Task CancelTimeslotAsync(OrderType orderType, string date, int? timeslotId)
        {
            if (!timeslotId.HasValue)
            {
                return;
            }

            if (orderType == OrderType.微信订票)
            {
                await CancelPublicTimeslotAsync(date, timeslotId.Value);
            }
            else if (orderType == OrderType.电话订票)
            {
                await CancelReservedTimeslotAsync(date, timeslotId.Value);
            }
        }

        private async Task BookPublicTimeslotAsync(string date, int timeslotId)
        {
            var success = await _explainerTimeslotSchedulingRepository.BookPublicTimeslotAsync(date, timeslotId);
            if (!success)
            {
                throw new UserFriendlyException("该场次已约满");
            }
        }

        private async Task CancelPublicTimeslotAsync(string date, int timeslotId)
        {
            await _explainerTimeslotSchedulingRepository.CancelPublicTimeslotAsync(date, timeslotId);
        }

        private async Task BookReservedTimeslotAsync(string date, int timeslotId)
        {
            var success = await _explainerTimeslotSchedulingRepository.BookReservedTimeslotAsync(date, timeslotId);
            if (!success)
            {
                throw new UserFriendlyException("该场次已约满");
            }
        }

        private async Task CancelReservedTimeslotAsync(string date, int timeslotId)
        {
            await _explainerTimeslotSchedulingRepository.CancelReservedTimeslotAsync(date, timeslotId);
        }

        public async Task BeginExplainAsync(string listNo, int explainerId, int timeslotId)
        {
            var workRecord = new ExplainerWorkRecord();
            workRecord.ListNo = listNo;
            workRecord.StaffId = explainerId;
            workRecord.TimeslotId = timeslotId;
            workRecord.BeginTime = DateTime.Now;

            await _explainerWorkRecordRepository.InsertAsync(workRecord);

            var eventData = new OrderExplainBeginingEventData();
            eventData.ListNo = listNo;
            eventData.ExplainerId = explainerId;

            await _eventBus.TriggerAsync(eventData);
        }

        public async Task CompleteExplainAsync(string listNo, int explainerId)
        {
            var workRecord = await _explainerWorkRecordRepository.FirstOrDefaultAsync(r => r.ListNo == listNo);
            if (workRecord == null)
            {
                throw new UserFriendlyException("该订单未开始讲解");
            }

            if (workRecord.StaffId != explainerId)
            {
                throw new UserFriendlyException("不能结束讲解他人的订单");
            }

            workRecord.CompleteTime = DateTime.Now;
        }
    }
}
