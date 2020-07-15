using Egoal.Domain.Entities;
using Egoal.TicketTypes;
using Egoal.UI;
using System;

namespace Egoal.Tickets
{
    public class TicketGroundCache : Entity<long>
    {
        public int GroundId { get; set; }
        public string TicketCode { get; set; }
        public string CardNo { get; set; }
        public string CertNo { get; set; }
        public bool? ValidFlag { get; set; }
        public TicketStatus TicketStatusId { get; set; }
        public FingerStatus? FingerStatusId { get; set; }
        public bool? PhotoBindFlag { get; set; }
        public long TicketId { get; set; }
        public Guid? TradeId { get; set; }
        public TicketTypeType? TicketTypeTypeId { get; set; }
        public int? TicketTypeId { get; set; }
        public CheckType? CheckTypeId { get; set; }
        public TicketKind? Tkid { get; set; }
        public int? ChangCiId { get; set; }
        public int? SeatId { get; set; }
        public string Stime { get; set; }
        public string Etime { get; set; }
        public int? TotalNum { get; set; }
        public int? SurplusNum { get; set; }
        public int? CheckInNum { get; set; }
        public int CheckOutNum { get; set; }
        public decimal? GroundPrice { get; set; }
        public Guid? MemberId { get; set; }
        public DateTime? Ctime { get; set; }
        public bool? LastIoflag { get; set; }
        public int? LastInGateId { get; set; }
        public DateTime? LastInCheckTime { get; set; }
        public int? LastOutGateId { get; set; }
        public DateTime? LastOutCheckTime { get; set; }
        public int? CheckTimesByDay { get; set; }
        public bool? DealFlag { get; set; }
        public bool? TimeoutFlag { get; set; }
        public bool? FirstActiveFlag { get; set; }
        public bool? SecondActiveFlag { get; set; }
        public long? Bid { get; set; }
        public bool? CommitFlag { get; set; } = true;
        public int? ParkId { get; set; }
        public Guid? SyncCode { get; set; } = Guid.NewGuid();

        public virtual TicketSale TicketSale { get; set; }

        public void Renew(TicketStatus ticketStatus, string etime)
        {
            ValidFlag = true;
            TicketStatusId = ticketStatus;
            Etime = etime;
        }

        public void Consume(int consumeNum, int? gateId)
        {
            if (CheckTypeId.Value.IsCheckByNum())
            {
                if (SurplusNum < consumeNum)
                {
                    throw new UserFriendlyException("次数已用完");
                }

                SurplusNum -= consumeNum;
                ValidFlag = SurplusNum > 0;
                if (!CheckInNum.HasValue)
                {
                    CheckInNum = 0;
                }
                CheckInNum += consumeNum;
            }
            if (!IsTodayUsed())
            {
                CheckTimesByDay = 0;
            }
            CheckTimesByDay += consumeNum;
            TicketStatusId = TicketStatus.已用;
            LastIoflag = true;
            LastInGateId = gateId;
            LastInCheckTime = DateTime.Now;
        }

        public void CheckOut(int checkNum, int? gateId)
        {
            if (CheckTypeId.Value.IsCheckByNum())
            {
                if (CheckOutNum + checkNum > TotalNum)
                {
                    throw new UserFriendlyException("次数已用完");
                }
            }

            LastIoflag = false;
            LastOutGateId = gateId;
            LastOutCheckTime = DateTime.Now;
            CheckOutNum += checkNum;
        }

        public bool IsTodayUsed()
        {
            return LastInCheckTime.HasValue && LastInCheckTime.Value.Date == DateTime.Now.Date;
        }
    }
}
