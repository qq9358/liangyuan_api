using Egoal.TicketTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.TicketTypes
{
    public class TicketTypeDescriptionMap : IEntityTypeConfiguration<TicketTypeDescription>
    {
        public void Configure(EntityTypeBuilder<TicketTypeDescription> entity)
        {
            entity.ToTable("TM_TicketTypeDescription");

            entity.Property(p => p.BookDescription)
                .IsRequired();
        }
    }
}
