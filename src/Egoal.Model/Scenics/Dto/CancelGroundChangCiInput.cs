namespace Egoal.Scenics.Dto
{
    public class CancelGroundChangCiInput
    {
        public bool HasGroundSeat { get; set; }
        public bool HasGroundChangCi { get; set; }
        public string ListNo { get; set; }
        public long? TicketId { get; set; }
        public int GroundId { get; set; }
        public string Date { get; set; }
        public int ChangCiId { get; set; }
        public int Quantity { get; set; }
        public bool IsRemote { get; set; }
    }
}
