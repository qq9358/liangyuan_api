namespace Egoal.Tickets.Dto
{
    public class CheckOutTicketInput
    {
        public int CheckNum { get; set; }
        public int? GroundId { get; set; }
        public int? GateGroupId { get; set; }
        public int? GateId { get; set; }
        public int? CheckerId { get; set; }
    }
}
