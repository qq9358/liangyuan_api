using Egoal.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Common
{
    public class AgeRangeMap : IEntityTypeConfiguration<AgeRange>
    {
        public void Configure(EntityTypeBuilder<AgeRange> entity)
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.ToTable("SM_AgeRange");
        }
    }
}
