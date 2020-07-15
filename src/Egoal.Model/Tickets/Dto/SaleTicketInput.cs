using Egoal.Extensions;
using Egoal.Scenics.Dto;
using Egoal.TicketTypes;
using Egoal.Trades;
using System;
using System.Collections.Generic;

namespace Egoal.Tickets.Dto
{
    public class SaleTicketInput
    {
        public SaleTicketInput()
        {
            TradeId = Guid.NewGuid();
            Ctime = DateTime.Now;

            Items = new List<SaleTicketItem>();
        }

        public SaleChannel SaleChannel { get; set; }
        public DateTime TravelDate { get; set; }
        public int? ChangCiId { get; set; }
        public Guid TradeId { get; set; }
        public string ListNo { get; set; }
        public TradeTypeType? TradeTypeTypeId { get; set; }
        public int? TradeTypeId { get; set; }
        public string TradeTypeName { get; set; }
        public TradeSource TradeSource { get; set; }
        public int? PayTypeId { get; set; }
        public string PayTypeName { get; set; }
        public bool PayFlag { get; set; }
        public Guid? MemberId { get; set; }
        public string MemberName { get; set; }
        public Guid? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid? GuiderId { get; set; }
        public string GuiderName { get; set; }
        public Guid? ManagerId { get; set; }
        public string ManagerName { get; set; }
        public int? AreaId { get; set; }
        public string AreaName { get; set; }
        public int? KeYuanTypeId { get; set; }
        public string KeYuanTypeName { get; set; }
        public string OrderListNo { get; set; }
        public decimal TotalMoney { get; set; }
        public bool BdFlag { get; set; }
        public bool? StatFlag { get; set; }
        public string Cdate { get; set; }
        public string Cweek { get; set; }
        public string Cmonth { get; set; }
        public string Cquarter { get; set; }
        public string Cyear { get; set; }
        public string Ctp { get; set; }
        public DateTime Ctime
        {
            get { return _ctime; }
            set
            {
                _ctime = value;

                Cdate = _ctime.ToDateString();
                Cweek = _ctime.ToWeekString();
                Cmonth = _ctime.ToMonthString();
                Cquarter = _ctime.ToQuarterString();
                Cyear = _ctime.ToYearString();
                Ctp = _ctime.ToHourString();
            }
        }
        private DateTime _ctime;
        public int? CashierId { get; set; }
        public string CashierName { get; set; }
        public int? CashPcid { get; set; }
        public string CashPcname { get; set; }
        public int? SalePointId { get; set; }
        public string SalePointName { get; set; }
        public int? SalesmanId { get; set; }
        public string SalesmanName { get; set; }
        public int? ParkId { get; set; }
        public string ParkName { get; set; }
        public bool IsExchange { get; set; }

        public List<SaleTicketItem> Items { get; set; }

        public List<TicketSaleSeatDto> Seats { get; set; }
    }

    public class SaleTicketItem
    {
        public SaleTicketItem()
        {
            Tourists = new List<TicketTourist>();
        }

        public int TicketTypeId { get; set; }
        public decimal TicPrice { get; set; }
        public decimal RealPrice { get; set; }
        public int Quantity { get; set; }
        public string CertNo { get; set; }
        public long? OrderDetailId { get; set; }
        public bool HasGroundSeat { get; set; }
        public List<GroundChangCiDto> GroundChangCis { get; set; }

        public List<TicketTourist> Tourists { get; set; }
    }

    public class TicketTourist
    {
        public string Name { get; set; }
        public string Birthday { get; set; }
        public string Mobile { get; set; }
        public int? CertType { get; set; }
        public string CertNo { get; set; }
    }
}
