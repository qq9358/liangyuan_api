using Egoal.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Tickets
{
    public class TicketSaleGroundSharingMap : IEntityTypeConfiguration<TicketSaleGroundSharing>
    {
        public void Configure(EntityTypeBuilder<TicketSaleGroundSharing> entity)
        {
            entity.ToTable("TM_TicketSaleGroundSharing");

            entity.HasIndex(e => e.GroundId)
                .HasName("IX_TicketSaleGroundSharing_GroundID");

            entity.HasIndex(e => e.TicketId)
                .HasName("IX_TicketSaleGroundSharing_TicketID");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.GroundId)
                .HasColumnName("GroundID");

            entity.Property(e => e.TicketId)
                .HasColumnName("TicketID");

            entity.HasOne(e => e.TicketSale)
                .WithMany(e => e.TicketSaleGroundSharings)
                .HasForeignKey(e => e.TicketId)
                .IsRequired();
        }
    }
}
