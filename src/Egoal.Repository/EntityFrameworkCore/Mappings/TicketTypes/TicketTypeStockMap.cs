using Egoal.TicketTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.TicketTypes
{
    public class TicketTypeStockMap : IEntityTypeConfiguration<TicketTypeStock>
    {
        public void Configure(EntityTypeBuilder<TicketTypeStock> entity)
        {
            entity.ToTable("TM_TicketTypeStock");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.CustomerId)
                .HasColumnName("CustomerID");

            entity.Property(e => e.EndDate)
                .HasColumnType("datetime");

            entity.Property(e => e.StartDate)
                .HasColumnType("datetime");

            entity.Property(e => e.TicketTypeId)
                .HasColumnName("TicketTypeID");
        }
    }
}
