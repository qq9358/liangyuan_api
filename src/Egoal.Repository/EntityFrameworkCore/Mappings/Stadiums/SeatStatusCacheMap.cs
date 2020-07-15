using Egoal.Stadiums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Stadiums
{
    public class SeatStatusCacheMap : IEntityTypeConfiguration<SeatStatusCache>
    {
        public void Configure(EntityTypeBuilder<SeatStatusCache> entity)
        {
            entity.ToTable("TM_TicketSaleSeatStatusCache");

            entity.HasIndex(e => e.ListNo)
                .HasName("IX_TicketSaleSeatStatusCache_ListNo");

            entity.HasIndex(e => e.Pcid)
                .HasName("IX_TicketSaleSeatStatusCache_PCID");

            entity.HasIndex(e => e.Sdate)
                .HasName("IX_TicketSaleSeatStatusCache_SDate");

            entity.HasIndex(e => e.SeatId)
                .HasName("IX_TicketSaleSeatStatus_SeatID");

            entity.HasIndex(e => e.TicketId)
                .HasName("IX_TicketSaleSeatStatusCache_TicketID");

            entity.HasIndex(e => e.TradeId)
                .HasName("IX_TicketSaleSeatStatusCache_TradeID");

            entity.HasIndex(e => new { e.SeatId, e.ChangCiId, e.Sdate })
                .HasName("IX_TicketSaleSeatStatusCache_Unique")
                .IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("ID")
                .HasColumnType("numeric(18, 0)")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.ChangCiId)
                .HasColumnName("ChangCiID");

            entity.Property(e => e.ListNo)
                .HasMaxLength(50);

            entity.Property(e => e.LockTime)
                .HasColumnType("datetime");

            entity.Property(e => e.Pcid)
                .HasColumnName("PCID");

            entity.Property(e => e.Sdate)
                .HasColumnName("SDate")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.SeatId)
                .HasColumnName("SeatID");

            entity.Property(e => e.StatusId)
                .HasColumnName("StatusID");

            entity.Property(e => e.TicketId)
                .HasColumnName("TicketID");

            entity.Property(e => e.TradeId)
                .HasColumnName("TradeID");
        }
    }
}
