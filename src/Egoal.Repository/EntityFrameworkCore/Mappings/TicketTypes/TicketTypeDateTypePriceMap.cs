using Egoal.TicketTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.TicketTypes
{
    public class TicketTypeDateTypePriceMap : IEntityTypeConfiguration<TicketTypeDateTypePrice>
    {
        public void Configure(EntityTypeBuilder<TicketTypeDateTypePrice> entity)
        {
            entity.HasKey(e => e.Id)
                    .ForSqlServerIsClustered(false);

            entity.ToTable("TM_TicketTypeDateTypePrice");

            entity.HasIndex(e => new { e.TicketTypeId, e.DateTypeId })
                .HasName("IX_TM_TicketTypeDateTypePrice")
                .IsUnique()
                .ForSqlServerIsClustered();

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.DateTypeId)
                .HasColumnName("DateTypeID");

            entity.Property(e => e.TicketTypeId)
                .HasColumnName("TicketTypeID");
        }
    }
}
