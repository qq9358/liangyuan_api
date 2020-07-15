using System;

namespace Egoal.Tickets.Dto
{
    public class StatTouristInput
    {
        public DateTime StartCTime { get; set; }
        public DateTime EndCTime { get; set; }
        public bool? StatByArea { get; set; }
        public bool? StatBySex { get; set; }
        public bool? StatByNation { get; set; }
        public bool? StatByAge { get; set; }
    }
}
