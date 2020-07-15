using Egoal.Domain.Entities;

namespace Egoal.Scenics
{
    public class GroundDateChangCiSaleNum : Entity
    {
        public int GroundId { get; set; }
        public string Date { get; set; }
        public int ChangCiId { get; set; }
        public int SaleNum { get; set; }
    }
}
