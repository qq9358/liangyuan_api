using System;
using System.Collections.Generic;
using System.Text;

namespace Egoal.Tickets.Dto
{
    public class StatTouristByAgeRangeInput
    {
        public DateTime? StartCTime { get; set; }
        public DateTime? EndCTime { get; set; }
        public int StatType { get; set; }
    }
}
