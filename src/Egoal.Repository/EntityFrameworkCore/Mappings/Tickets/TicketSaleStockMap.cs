using Egoal.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Tickets
{
    public class TicketSaleStockMap : IEntityTypeConfiguration<TicketSaleStock>
    {
        public void Configure(EntityTypeBuilder<TicketSaleStock> entity)
        {
            entity.ToTable("TM_TicketSaleStock");

            entity.HasIndex(e => e.TravelDate);

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.CustomerId)
                .HasColumnName("CustomerID");

            entity.Property(e => e.TicketTypeId)
                .HasColumnName("TicketTypeID");

            entity.Property(e => e.TravelDate)
                .HasColumnType("datetime");
        }
    }
}
