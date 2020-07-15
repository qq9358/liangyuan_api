using Egoal.TicketTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.TicketTypes
{
    public class TicketTypeAgeRangeMap : IEntityTypeConfiguration<TicketTypeAgeRange>
    {
        public void Configure(EntityTypeBuilder<TicketTypeAgeRange> entity)
        {
            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.TicketTypeId)
                .HasColumnName("TicketTypeID");

            entity.ToTable("TM_TicketTypeAgeRange");
        }
    }
}
