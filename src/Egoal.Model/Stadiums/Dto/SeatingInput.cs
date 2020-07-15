using System;

namespace Egoal.Stadiums.Dto
{
    public class SeatingInput
    {
        public string ListNo { get; set; }
        public int StadiumId { get; set; }
        public string Date { get; set; }
        public int ChangCiId { get; set; }
        public int Quantity { get; set; }
        public int LockMinutes { get; set; }
    }
}
