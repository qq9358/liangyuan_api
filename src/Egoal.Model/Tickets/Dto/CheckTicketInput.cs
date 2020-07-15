using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.Tickets.Dto
{
    public class CheckTicketInput
    {
        [Display(Name = "票号")]
        [MustFillIn]
        [MaximumLength(50)]
        public string TicketCode { get; set; }
        public int GroundId { get; set; }
        public int GateGroupId { get; set; }
        public int GateId { get; set; }
        public ConsumeType ConsumeType { get; set; }
    }
}
