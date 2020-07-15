using Egoal.Domain.Entities;

namespace Egoal.TicketTypes
{
    public class TicketTypeAgeRange : Entity
    {
        public int TicketTypeId { get; set; }
        public int StartAge { get; set; }
        public int EndAge { get; set; }

        /// <summary>
        /// 转对外数据类型
        /// </summary>
        /// <returns></returns>
        public Dto.TicketTypeAgeRange ToDtoTicketTypeAgeRange()
        {
            Dto.TicketTypeAgeRange dtoTicketTypeAgeRange = new Dto.TicketTypeAgeRange();
            dtoTicketTypeAgeRange.TicketTypeId = TicketTypeId;
            dtoTicketTypeAgeRange.StartAge = StartAge;
            dtoTicketTypeAgeRange.EndAge = EndAge;
            return dtoTicketTypeAgeRange;
        }
    }
}
