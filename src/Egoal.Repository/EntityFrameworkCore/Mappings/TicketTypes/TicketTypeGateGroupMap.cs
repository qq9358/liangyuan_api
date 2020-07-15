using Egoal.TicketTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.TicketTypes
{
    public class TicketTypeGateGroupMap : IEntityTypeConfiguration<TicketTypeGateGroup>
    {
        public void Configure(EntityTypeBuilder<TicketTypeGateGroup> entity)
        {
            entity.HasKey(e => e.Id)
                    .ForSqlServerIsClustered(false);

            entity.ToTable("TM_TicketTypeGateGroup");

            entity.HasIndex(e => new { e.TicketTypeId, e.GateGroupId })
                .HasName("IX_TM_TicketTypeGateGroup")
                .IsUnique()
                .ForSqlServerIsClustered();

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.GateGroupId)
                .HasColumnName("GateGroupID");

            entity.Property(e => e.TicketTypeId)
                .HasColumnName("TicketTypeID");
        }
    }
}
