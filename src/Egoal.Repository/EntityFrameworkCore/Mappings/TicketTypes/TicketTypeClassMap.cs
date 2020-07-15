using Egoal.TicketTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.TicketTypes
{
    public class TicketTypeClassMap : IEntityTypeConfiguration<TicketTypeClass>
    {
        public void Configure(EntityTypeBuilder<TicketTypeClass> entity)
        {
            entity.ToTable("TM_TicketTypeClass");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.SortCode)
                .HasMaxLength(50);
        }
    }
}
