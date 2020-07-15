using Egoal.Dependency;
using Egoal.Extensions;
using Egoal.TicketTypes;
using Egoal.UI;
using System;
using System.Threading.Tasks;

namespace Egoal.Tickets
{
    /// <summary>
    /// 有效期计算
    /// </summary>
    public class ExpirationDateCalculator : IScopedDependency
    {
        private readonly ITicketTypeRepository _ticketTypeRepository;

        public ExpirationDateCalculator(ITicketTypeRepository ticketTypeRepository)
        {
            _ticketTypeRepository = ticketTypeRepository;
        }

        public DateTime GetStartValidTime(DateTime travelDate, TicketType ticketType)
        {
            var startDate = travelDate;
            DateTime? startValidDate = ticketType.SvalidDate.IsNullOrEmpty() ? (DateTime?)null : ticketType.SvalidDate.To<DateTime>();
            if (startValidDate.HasValue && startDate.Date < startValidDate.Value.Date)
            {
                startDate = startValidDate.Value.Date;
            }

            string startTime = $"{startDate.ToDateString()} {ticketType.Stime}:00";

            return startTime.To<DateTime>();
        }

        public DateTime GetEndValidTime(DateTime travelDate, TicketType ticketType)
        {
            var endDate = travelDate;
            if (ticketType.Days.HasValue)
            {
                endDate = endDate.AddDays(ticketType.Days.Value - 1);
            }

            DateTime? endValidDate = ticketType.ValidDate.IsNullOrEmpty() ? (DateTime?)null : ticketType.ValidDate.To<DateTime>();
            if (endValidDate.HasValue && endDate.Date > endValidDate.Value.Date)
            {
                endDate = endValidDate.Value.Date;
            }

            string endTime = $"{endDate.ToDateString()} {ticketType.Etime}:59";

            return endTime.To<DateTime>();
        }

        public async Task<DateTime> GetEndDelayTimeAsync(DateTime travelDate, int ticketTypeId)
        {
            var ticketType = await _ticketTypeRepository.GetAsync(ticketTypeId);

            return GetEndDelayTime(travelDate, ticketType);
        }

        public DateTime GetEndDelayTime(DateTime travelDate, TicketType ticketType)
        {
            if (!ticketType.DelayDays.HasValue)
            {
                throw new UserFriendlyException("延期天数未设置");
            }

            var endDate = travelDate.AddDays(ticketType.DelayDays.Value - 1);

            DateTime? endValidDate = ticketType.ValidDate.IsNullOrEmpty() ? (DateTime?)null : ticketType.ValidDate.To<DateTime>();
            if (endValidDate.HasValue && endDate.Date > endValidDate.Value.Date)
            {
                endDate = endValidDate.Value.Date;
            }

            string endTime = $"{endDate.ToDateString()} {ticketType.Etime}:59";

            return endTime.To<DateTime>();
        }
    }
}
