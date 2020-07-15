using Egoal.Tickets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Egoal.Members
{
    public static class MemberExtensions
    {
        public static MemberCard MapToMemberCard(this TicketSale ticketSale)
        {
            var memberCard = new MemberCard();
            memberCard.MemberId = ticketSale.MemberId;
            memberCard.TradeId = ticketSale.TradeId;
            memberCard.ListNo = ticketSale.ListNo;
            memberCard.TicketId = ticketSale.Id;
            memberCard.TicketCode = ticketSale.TicketCode;
            memberCard.CardNo = ticketSale.CardNo;
            memberCard.Stime = ticketSale.Stime.Substring(0, 10);
            memberCard.Etime = ticketSale.Etime.Substring(0, 10);
            memberCard.TicketTypeId = ticketSale.TicketTypeId;
            memberCard.TicketTypeName = ticketSale.TicketTypeName;
            memberCard.TicketStatusId = (int)ticketSale.TicketStatusId;
            memberCard.TicketStatusName = ticketSale.TicketStatusName;
            memberCard.CardValidFlag = ticketSale.ValidFlag;
            memberCard.CardValidFlagName = ticketSale.ValidFlagName;

            return memberCard;
        }
    }
}
