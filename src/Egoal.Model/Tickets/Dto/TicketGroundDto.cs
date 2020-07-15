using Egoal.Application.Services.Dto;

namespace Egoal.Tickets.Dto
{
    public class TicketGroundDto : EntityDto<long>
    {
        public string GroundName { get; set; }
        public string ChangCiName { get; set; }
        public int SurplusNum { get; set; }
        public string Stime { get; set; }
        public string Etime { get; set; }
    }
}
