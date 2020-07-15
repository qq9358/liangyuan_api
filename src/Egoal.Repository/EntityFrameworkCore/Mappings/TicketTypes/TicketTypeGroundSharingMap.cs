using Egoal.TicketTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.TicketTypes
{
    public class TicketTypeGroundSharingMap : IEntityTypeConfiguration<TicketTypeGroundSharing>
    {
        public void Configure(EntityTypeBuilder<TicketTypeGroundSharing> entity)
        {
            entity.ToTable("TM_TicketTypeGroundSharing");

            entity.HasIndex(e => e.GroundId)
                .HasName("IX_TicketTypeGroundSharing_GroundID");

            entity.HasIndex(e => e.TicketTypeId)
                .HasName("IX_TicketTypeGroundSharing_TicketTypeID");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.GroundId)
                .HasColumnName("GroundID");

            entity.Property(e => e.TicketTypeId)
                .HasColumnName("TicketTypeID");

            entity.Property(e => e.DateTypeId)
                .HasColumnName("DateTypeID");

            entity.HasOne(e => e.TicketType)
                .WithMany(e => e.TicketTypeGroundSharings)
                .HasForeignKey(e => e.TicketTypeId);
        }
    }
}
