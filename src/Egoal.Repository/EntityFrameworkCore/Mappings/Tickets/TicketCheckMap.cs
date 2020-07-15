using Egoal.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Tickets
{
    public class TicketCheckMap : IEntityTypeConfiguration<TicketCheck>
    {
        public void Configure(EntityTypeBuilder<TicketCheck> entity)
        {
            entity.HasKey(e => e.Id)
                    .ForSqlServerIsClustered(false);

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Bid)
                .HasColumnName("BID");

            entity.Property(e => e.CardNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CashPcid)
                .HasColumnName("CashPCID");

            entity.Property(e => e.CashPcname)
                .HasColumnName("CashPCName")
                .HasMaxLength(50);

            entity.Property(e => e.CashierId)
                .HasColumnName("CashierID");

            entity.Property(e => e.CashierName)
                .HasMaxLength(50);

            entity.Property(e => e.Cdate)
                .HasColumnName("CDate")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.CheckTypeId)
                .HasColumnName("CheckTypeID");

            entity.Property(e => e.CheckTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.CheckerId)
                .HasColumnName("CheckerID");

            entity.Property(e => e.CheckerName)
                .HasMaxLength(50);

            entity.Property(e => e.Cmonth)
                .HasColumnName("CMonth")
                .HasMaxLength(7)
                .IsUnicode(false);

            entity.Property(e => e.ConsumeMinutes)
                .HasMaxLength(50);

            entity.Property(e => e.Cquarter)
                .HasColumnName("CQuarter")
                .HasMaxLength(6)
                .IsUnicode(false);

            entity.Property(e => e.Ctime)
                .HasColumnName("CTime")
                .HasColumnType("datetime");

            entity.Property(e => e.Ctp)
                .HasColumnName("CTP")
                .HasMaxLength(5)
                .IsUnicode(false);

            entity.Property(e => e.CustomerId)
                .HasColumnName("CustomerID");

            entity.Property(e => e.CustomerName)
                .HasMaxLength(50);

            entity.Property(e => e.Cweek)
                .HasColumnName("CWeek")
                .HasMaxLength(3);

            entity.Property(e => e.Cyear)
                .HasColumnName("CYear")
                .HasMaxLength(4)
                .IsUnicode(false);

            entity.Property(e => e.DeptId)
                .HasColumnName("DeptID");

            entity.Property(e => e.DeptName)
                .HasMaxLength(50);

            entity.Property(e => e.Etime)
                .HasColumnName("ETime")
                .HasMaxLength(19)
                .IsUnicode(false);

            entity.Property(e => e.FxCardNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.FxTicketCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.GateGroupId)
                .HasColumnName("GateGroupID");

            entity.Property(e => e.GateGroupName)
                .HasMaxLength(50);

            entity.Property(e => e.GateId)
                .HasColumnName("GateID");

            entity.Property(e => e.GateName)
                .HasMaxLength(50);

            entity.Property(e => e.GlkOwnerId)
                .HasColumnName("GlkOwnerID");

            entity.Property(e => e.GlkOwnerName)
                .HasMaxLength(50);

            entity.Property(e => e.GroundId)
                .HasColumnName("GroundID");

            entity.Property(e => e.GroundName)
                .HasMaxLength(50);

            entity.Property(e => e.GroundPlayTypeId)
                .HasColumnName("GroundPlayTypeID");

            entity.Property(e => e.GroundPlayTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.GuiderId)
                .HasColumnName("GuiderID");

            entity.Property(e => e.GuiderName)
                .HasMaxLength(50);

            entity.Property(e => e.InOutFlagName)
                .HasMaxLength(1);

            entity.Property(e => e.ListNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Ltime)
                .HasColumnName("LTime")
                .HasColumnType("datetime");

            entity.Property(e => e.MemberId)
                .HasColumnName("MemberID");

            entity.Property(e => e.MemberName)
                .HasMaxLength(50);

            entity.Property(e => e.Memo)
                .HasMaxLength(200);

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.ParkName)
                .HasMaxLength(50);

            entity.Property(e => e.RecycleFlagName)
                .HasMaxLength(1);

            entity.Property(e => e.SaleParkId)
                .HasColumnName("SaleParkID");

            entity.Property(e => e.SaleParkName)
                .HasMaxLength(50);

            entity.Property(e => e.SalePointId)
                .HasColumnName("SalePointID");

            entity.Property(e => e.SalePointName)
                .HasMaxLength(50);

            entity.Property(e => e.Stime)
                .HasColumnName("STime")
                .HasMaxLength(19)
                .IsUnicode(false);

            entity.Property(e => e.TicketCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.TicketId)
                .HasColumnName("TicketID");

            entity.Property(e => e.TicketTypeId)
                .HasColumnName("TicketTypeID");

            entity.Property(e => e.TicketTypeName)
                .HasMaxLength(50);

            entity.Property(e => e.TicketTypeTypeId)
                .HasColumnName("TicketTypeTypeID");

            entity.Property(e => e.TradeId)
                .HasColumnName("TradeID");

            entity.Property(e => e.UniqueId)
                .HasColumnName("UniqueID");

            entity.ToTable("TM_TicketCheck");

            entity.HasIndex(e => e.CardNo);

            entity.HasIndex(e => e.Ctime)
                .ForSqlServerIsClustered();

            entity.HasIndex(e => e.SyncCode);

            entity.HasIndex(e => e.TicketCode);

            entity.HasIndex(e => e.TicketId);

            entity.HasIndex(e => e.UniqueId);
        }
    }
}
