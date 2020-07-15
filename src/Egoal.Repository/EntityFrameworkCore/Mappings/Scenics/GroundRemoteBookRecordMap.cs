using Egoal.Scenics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Egoal.EntityFrameworkCore.Mappings.Scenics
{
    public class GroundRemoteBookRecordMap : IEntityTypeConfiguration<GroundRemoteBookRecord>
    {
        public void Configure(EntityTypeBuilder<GroundRemoteBookRecord> entity)
        {
            entity.ToTable("OM_GroundRemoteBookRecord");

            entity.Property(e => e.ListNo)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Date)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);
        }
    }
}
