using System.Collections.Generic;

namespace Egoal.Scenics.Dto
{
    public class BookGroundChangCiOutput
    {
        public bool HasGroundSeat { get; set; }
        public bool HasGroundChangCi { get; set; }
        public int GroundId { get; set; }
        public string GroundName { get; set; }
        public int ChangCiId { get; set; }
        public string ChangCiName { get; set; }
        public List<NameValue<long>> Seats { get; set; }
    }
}
