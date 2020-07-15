using Egoal.Domain.Entities;

namespace Egoal.Scenics
{
    public class Ground : Entity
    {
        public string Name { get; set; }
        public string SortCode { get; set; }
        public int? GroundTypeId { get; set; }
        public int? GroundPlayTypeId { get; set; }
        public int? DeptId { get; set; }
        public int? ParkId { get; set; }
        public int? MaxCheckNumByDay { get; set; }
        public int? LastGroundId { get; set; }
        public int? StadiumId { get; set; }
        public int? ExtraGateGroupId { get; set; }
        public decimal? TicPrice { get; set; }
        public int? CzkPayMode { get; set; }
        public bool? OnlineCheck { get; set; }
        public bool JsFlag { get; set; }
        public int? InOutControlType { get; set; }
        public bool? SeatSaleFlag { get; set; }
        public bool? ChangCiSaleFlag { get; set; }
        public int? ChangCiDelaySaleMinutes { get; set; }
        public int? SeatNum { get; set; }
        public int? DefaultTicketTypeId { get; set; }
        public int? FeeUnit { get; set; }
        public decimal? MinFee { get; set; }
        public decimal? MaxFee { get; set; }
        public decimal? YaJin { get; set; }
        public int? DailySaleNumPerMember { get; set; }
        public bool? SaleFlag { get; set; }
    }
}
