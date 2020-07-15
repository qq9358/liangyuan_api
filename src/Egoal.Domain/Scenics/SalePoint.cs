using Egoal.Domain.Entities;
using System;

namespace Egoal.Scenics
{
    public class SalePoint : Entity
    {
        public string Name { get; set; }
        public SalePointType? SalePointType { get; set; }
        public string SortCode { get; set; }
        public int? ParkId { get; set; }
        public Guid SyncCode { get; set; }
    }
}
