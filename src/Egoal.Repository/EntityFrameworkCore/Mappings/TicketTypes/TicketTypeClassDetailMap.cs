using Egoal.TicketTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.TicketTypes
{
    public class TicketTypeClassDetailMap : IEntityTypeConfiguration<TicketTypeClassDetail>
    {
        public void Configure(EntityTypeBuilder<TicketTypeClassDetail> entity)
        {
            entity.HasKey(e => e.Id)
                    .ForSqlServerIsClustered(false);

            entity.ToTable("TM_TicketTypeClassDetail");

            entity.HasIndex(e => new { e.TicketTypeClassId, e.TicketTypeId })
                .HasName("IX_TM_TicketTypeClassDetail")
                .IsUnique()
                .ForSqlServerIsClustered();

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.TicketTypeClassId)
                .HasColumnName("TicketTypeClassID");

            entity.Property(e => e.TicketTypeId)
                .HasColumnName("TicketTypeID");
        }
    }
}
