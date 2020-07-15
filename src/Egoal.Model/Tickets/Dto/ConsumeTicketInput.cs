namespace Egoal.Tickets.Dto
{
    public class ConsumeTicketInput
    {
        public int ConsumeNum { get; set; }
        public ConsumeType ConsumeType { get; set; }
        public int? GroundId { get; set; }
        public int? GateGroupId { get; set; }
        public int? GateId { get; set; }
        public int? CheckerId { get; set; }
    }
}
