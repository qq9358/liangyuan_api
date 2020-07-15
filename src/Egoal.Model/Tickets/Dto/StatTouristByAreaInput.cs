using System;
using System.Collections.Generic;
using System.Text;

namespace Egoal.Tickets.Dto
{
    public class StatTouristByAreaInput
    {
        public DateTime StartCTime { get; set; }
        public DateTime EndCTime { get; set; }
        public string ProvinceName { get; set; }
        public int StatType { get; set; }
    }
}
