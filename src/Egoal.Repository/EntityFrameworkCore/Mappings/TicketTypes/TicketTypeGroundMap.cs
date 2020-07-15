using Egoal.TicketTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.TicketTypes
{
    public class TicketTypeGroundMap : IEntityTypeConfiguration<TicketTypeGround>
    {
        public void Configure(EntityTypeBuilder<TicketTypeGround> entity)
        {
            entity.HasKey(e => e.Id)
                    .ForSqlServerIsClustered(false);

            entity.ToTable("TM_TicketTypeGround");

            entity.HasIndex(e => new { e.TicketTypeId, e.GroundId })
                .HasName("IX_TM_TicketTypeGround")
                .IsUnique()
                .ForSqlServerIsClustered();

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.CheckTypeId)
                .HasColumnName("CheckTypeID");

            entity.Property(e => e.GroundId)
                .HasColumnName("GroundID");

            entity.Property(e => e.Preferred)
                .HasDefaultValueSql("((1))");

            entity.Property(e => e.TicketTypeId)
                .HasColumnName("TicketTypeID");

            entity.HasOne(e => e.TicketType)
                .WithMany(t => t.TicketTypeGrounds)
                .HasForeignKey(e => e.TicketTypeId)
                .IsRequired();
        }
    }
}
