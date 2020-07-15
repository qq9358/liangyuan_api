using Egoal.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Tickets
{
    public class TicketGroundCacheMap : IEntityTypeConfiguration<TicketGroundCache>
    {
        public void Configure(EntityTypeBuilder<TicketGroundCache> entity)
        {
            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Bid)
                .HasColumnName("BID");

            entity.Property(e => e.CardNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CertNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.ChangCiId)
                .HasColumnName("ChangCiID");

            entity.Property(e => e.SeatId)
                .HasColumnName("SeatID");

            entity.Property(e => e.CheckTypeId)
                .HasColumnName("CheckTypeID");

            entity.Property(e => e.Ctime)
                .HasColumnName("CTime")
                .HasColumnType("datetime");

            entity.Property(e => e.Etime)
                .HasColumnName("ETime")
                .HasMaxLength(19)
                .IsUnicode(false);

            entity.Property(e => e.FingerStatusId)
                .HasColumnName("FingerStatusID");

            entity.Property(e => e.GroundId)
                .HasColumnName("GroundID");

            entity.Property(e => e.LastInCheckTime)
                .HasColumnType("datetime");

            entity.Property(e => e.LastInGateId)
                .HasColumnName("LastInGateID");

            entity.Property(e => e.LastIoflag)
                .HasColumnName("LastIOFlag");

            entity.Property(e => e.LastOutCheckTime)
                .HasColumnType("datetime");

            entity.Property(e => e.LastOutGateId)
                .HasColumnName("LastOutGateID");

            entity.Property(e => e.MemberId)
                .HasColumnName("MemberID");

            entity.Property(e => e.ParkId)
                .HasColumnName("ParkID");

            entity.Property(e => e.Stime)
                .HasColumnName("STime")
                .HasMaxLength(19)
                .IsUnicode(false);

            entity.Property(e => e.TicketCode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.TicketId)
                .HasColumnName("TicketID");

            entity.Property(e => e.TicketStatusId)
                .HasColumnName("TicketStatusID");

            entity.Property(e => e.TicketTypeId)
                .HasColumnName("TicketTypeID");

            entity.Property(e => e.TicketTypeTypeId)
                .HasColumnName("TicketTypeTypeID");

            entity.Property(e => e.Tkid)
                .HasColumnName("TKID");

            entity.Property(e => e.TradeId)
                .HasColumnName("TradeID");

            entity.Property(e => e.SurplusNum)
                .IsConcurrencyToken();

            entity.ToTable("TM_TicketGroundCache");

            entity.HasIndex(e => e.CardNo);

            entity.HasIndex(e => e.CertNo);

            entity.HasIndex(e => e.SyncCode);

            entity.HasIndex(e => e.TicketCode);

            entity.HasIndex(e => e.TicketId);

            entity.HasIndex(e => e.TradeId);

            entity.HasOne(e => e.TicketSale)
                .WithMany(t => t.TicketGroundCaches)
                .HasForeignKey(e => e.TicketId)
                .IsRequired();
        }
    }
}
