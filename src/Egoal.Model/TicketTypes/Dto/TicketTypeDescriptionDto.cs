using Egoal.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Egoal.TicketTypes.Dto
{
    public class TicketTypeDescriptionDto
    {
        public int TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }

        [Display(Name = "预订说明")]
        [MustFillIn]
        public string BookDescription { get; set; }
        public string FeeDescription { get; set; }
        public string UsageDescription { get; set; }
        public string RefundDescription { get; set; }
        public string OtherDescription { get; set; }
    }
}
