using Egoal.Scenics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Scenics
{
    public class ParkMap : IEntityTypeConfiguration<Park>
    {
        public void Configure(EntityTypeBuilder<Park> entity)
        {
            entity.ToTable("VM_Park");

            entity.Property(e => e.Id)
                .HasColumnName("ID")
                .ValueGeneratedNever();

            entity.Property(e => e.Name)
                .HasMaxLength(50);

            entity.Property(e => e.SortCode)
                .HasMaxLength(50);
        }
    }
}
