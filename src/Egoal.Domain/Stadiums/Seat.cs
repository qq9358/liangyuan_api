using Egoal.Domain.Entities;
using System;

namespace Egoal.Stadiums
{
    public class Seat : Entity<long>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string GroupCode { get; set; }
        public int? StadiumTypeId { get; set; }
        public int? StadiumId { get; set; }
        public int? RegionId { get; set; }
        public int? SeatTypeId { get; set; }
        public int? PersonNum { get; set; }
        public bool? JzFlag { get; set; }
        public int? RowCode { get; set; }
        public int? ColumnCode { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }
        public int? Xlen { get; set; }
        public int? Ylen { get; set; }
        public bool? ValidFlag { get; set; }
        public bool? PrioritySaleFlag { get; set; }
        public int? Bid { get; set; }
        public bool? LockFlag { get; set; }
        public DateTime? LockTime { get; set; }
        public int? Lbid { get; set; }
        public int? Lbid2 { get; set; }
    }
}
