using Egoal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Egoal.Common
{
    public class TmDate : Entity
    {
        public string Date { get; set; }
        public int? DateTypeId { get; set; }
        public int? PriceTypeId { get; set; }
        public int? ParkId { get; set; }
        public Guid SyncCode { get; set; }
    }
}
