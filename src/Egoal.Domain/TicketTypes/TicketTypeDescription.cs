using Egoal.Domain.Entities;

namespace Egoal.TicketTypes
{
    public class TicketTypeDescription : Entity
    {
        public int TicketTypeId { get; set; }
        public string BookDescription { get; set; }
        public string FeeDescription { get; set; }
        public string UsageDescription { get; set; }
        public string RefundDescription { get; set; }
        public string OtherDescription { get; set; }

        public virtual TicketType TicketType { get; set; }
    }
}
