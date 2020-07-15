using Egoal.Domain.Entities;
using System;

namespace Egoal.Scenics
{
    public class GroundRemoteBookRecord : Entity<long>
    {
        public string ListNo { get; set; }
        public string Date { get; set; }
        public int GroundId { get; set; }
        public int ChangCiId { get; set; }
        public int Quantity { get; set; }
        public bool IsCanceled { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
