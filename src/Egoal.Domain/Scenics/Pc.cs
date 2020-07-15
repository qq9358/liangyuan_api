using Egoal.Domain.Entities;
using System;

namespace Egoal.Scenics
{
    public class Pc : Entity
    {
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Mac { get; set; }
        public int? SalePointId { get; set; }
        public int? WareShopId { get; set; }
        public string WareShopName { get; set; }
        public bool? PermitFlag { get; set; } = false;
        public string IdentifyCode { get; set; }
        public string Memo { get; set; }
        public int? Cid { get; set; }
        public DateTime? Ctime { get; set; } = DateTime.Now;
        public int? Mid { get; set; }
        public DateTime? Mtime { get; set; }
        public int? ParkId { get; set; }
        public int? OnlinePayParamId { get; set; }
        public Guid SyncCode { get; set; } = Guid.NewGuid();
    }
}
