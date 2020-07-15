using Egoal.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Tickets
{
    public class TicketSaleSeatMap : IEntityTypeConfiguration<TicketSaleSeat>
    {
        public void Configure(EntityTypeBuilder<TicketSaleSeat> entity)
        {
            entity.ToTable("TM_TicketSaleSeat");

            entity.HasIndex(e => e.Sdate)
                .HasName("IX_TicketSaleSeat_SDate");

            entity.HasIndex(e => e.SeatId)
                .HasName("IX_TicketSaleSeat_SeatID");

            entity.HasIndex(e => e.TicketId)
                .HasName("IX_TicketSaleSeat_TicketID");

            entity.HasIndex(e => e.TradeId)
                .HasName("IX_TicketSaleSeat_TradeID");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.ChangCiId)
                .HasColumnName("ChangCiID");

            entity.Property(e => e.Sdate)
                .HasColumnName("SDate")
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.SeatId)
                .HasColumnName("SeatID");

            entity.Property(e => e.TicketId)
                .HasColumnName("TicketID");

            entity.Property(e => e.TradeId)
                .HasColumnName("TradeID");

            entity.HasOne(e => e.TicketSale)
                .WithMany(e => e.TicketSaleSeats)
                .HasForeignKey(e => e.TicketId);
        }
    }
}
